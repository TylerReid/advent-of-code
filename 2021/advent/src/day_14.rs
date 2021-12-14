use std::{collections::HashMap, task::Poll, ops::RangeBounds};

pub fn f() {
    let input = std::fs::read_to_string("input/14").unwrap();

    let (t, r) = input.split_once("\n\n").unwrap();

    let mut polymer = parse_polymer(t);
    let rules = parse_rules(r);

    for _ in 0..10 {
        polymer = step(&rules, polymer);
    }

    // slice group by is not in stable :(
    let mut counts = HashMap::new();
    for p in polymer {
        if let Some(x) = counts.get_mut(&p) {
            *x += 1;
        } else {
            counts.insert(p, 1);
        }
    }

    let mut c: Vec<(&char, &i32)> = counts.iter().collect();
    c.sort_by(|a, b| a.1.cmp(b.1));

    println!("{:?}", c);
    println!("{}", c.last().unwrap().1 - c.first().unwrap().1);
}

type Rules = HashMap<(char, char), char>;
type Polymer = Vec<char>;

fn step(r: &Rules, p: Polymer) -> Polymer {
    let mut new_polymer = Vec::new();
    new_polymer.push(p[0]);
    for i in 1..p.len() {
        if let Some(v) = r.get(&(p[i-1], p[i])) {
            new_polymer.push(*v);
        }
        new_polymer.push(p[i]);
    }
    new_polymer
}

fn parse_polymer(s: &str) -> Polymer {
    s.chars().collect()
}

fn parse_rules(s: &str) -> Rules {
    let mut h = HashMap::new();
    for line in s.lines() {
        let (k, v) = line.split_once(" -> ").unwrap();
        h.insert((k.chars().nth(0).unwrap(), k.chars().nth(1).unwrap()), v.chars().nth(0).unwrap());
    }
    h
}