use std::{cmp::Ordering, collections::BinaryHeap};

use super::input;

pub fn f() {
    let mut input: Vec<Vec<usize>> = std::fs::read_to_string("input/15")
        .unwrap()
        .lines()
        .map(|s| s.chars().map(|c| c.to_string().parse().unwrap()).collect())
        .collect();

    let original_stride: usize = 100;
    let stride = original_stride * 5; //todo sqrt of input len?
    let length = stride * stride - 1;

    for line in input.iter_mut() {
        let mut new_costs = Vec::new();
        for i in 1..9 {
            for cost in line.iter() {
                let c = (i + *cost) % 9;
                new_costs.push(if c == 0 { 9 } else { c });
            }
        }
        line.append(&mut new_costs);
    }

    let mut derp: Vec<Vec<usize>> = Vec::new();
    for i in 0..5 {
        for line in input.iter() {
            let mut new_line = Vec::new();
            for cost in line.iter().skip(i * original_stride) {
                new_line.push(*cost);
            }
            derp.push(new_line);
        }
    }

    let mut final_input = Vec::new();
    for x in 0..stride {
        for y in 0..stride {
            final_input.push(derp[x][y]);
        }
    }

    

    let mut nodes = Vec::new();
    for position in 0..=length {
        let mut edges = Vec::new();
        //top
        if let Some(v) = position.checked_sub(stride) {
            edges.push(Edge {
                node: v,
                cost: final_input[v],
            });
        }
        //bottom
        if position + stride < length {
            edges.push(Edge {
                node: position + stride,
                cost: final_input[position + stride],
            });
        }
        //left
        if position % stride != 0 {
            edges.push(Edge {
                node: position - 1,
                cost: final_input[position - 1],
            });
        }
        //right
        if position % stride != stride - 1 {
            edges.push(Edge {
                node: position + 1,
                cost: final_input[position + 1],
            });
        }

        nodes.push(edges);
    }

    //println!("{:#?}", nodes);

    let cost = shortest_path(&nodes, 0, length);

    println!("{:?}", cost);
}

// Looked up the priority queue implementation in rust, and it just had this whole example worked out
// couldn't be bothered to do the work to re-implement it after understanding how it works

#[derive(Copy, Clone, Eq, PartialEq)]
struct State {
    cost: usize,
    position: usize,
}

// The priority queue depends on `Ord`.
// Explicitly implement the trait so the queue becomes a min-heap
// instead of a max-heap.
impl Ord for State {
    fn cmp(&self, other: &Self) -> Ordering {
        // Notice that the we flip the ordering on costs.
        // In case of a tie we compare positions - this step is necessary
        // to make implementations of `PartialEq` and `Ord` consistent.
        other
            .cost
            .cmp(&self.cost)
            .then_with(|| self.position.cmp(&other.position))
    }
}

// `PartialOrd` needs to be implemented as well.
impl PartialOrd for State {
    fn partial_cmp(&self, other: &Self) -> Option<Ordering> {
        Some(self.cmp(other))
    }
}

// Each node is represented as a `usize`, for a shorter implementation.
#[derive(Debug)]
struct Edge {
    node: usize,
    cost: usize,
}

fn shortest_path(adj_list: &Vec<Vec<Edge>>, start: usize, goal: usize) -> Option<usize> {
    // dist[node] = current shortest distance from `start` to `node`
    let mut dist: Vec<_> = (0..adj_list.len()).map(|_| usize::MAX).collect();

    let mut heap = BinaryHeap::new();

    // We're at `start`, with a zero cost
    dist[start] = 0;
    heap.push(State {
        cost: 0,
        position: start,
    });

    // Examine the frontier with lower cost nodes first (min-heap)
    while let Some(State { cost, position }) = heap.pop() {
        // Alternatively we could have continued to find all shortest paths
        if position == goal {
            return Some(cost);
        }

        // Important as we may have already found a better way
        if cost > dist[position] {
            continue;
        }

        // For each node we can reach, see if we can find a way with
        // a lower cost going through this node
        for edge in &adj_list[position] {
            let next = State {
                cost: cost + edge.cost,
                position: edge.node,
            };

            // If so, add it to the frontier and continue
            if next.cost < dist[next.position] {
                heap.push(next);
                // Relaxation, we have now found a better way
                dist[next.position] = next.cost;
            }
        }
    }

    // Goal not reachable
    None
}
