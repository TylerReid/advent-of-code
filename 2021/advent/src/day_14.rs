use std::{collections::HashMap, task::Poll, ops::RangeBounds};

pub fn f() {
    let input = std::fs::read_to_string("input/14").unwrap();

    let (t, r) = input.split_once("\n\n").unwrap();

    let mut polymer = parse_polymer(t);
    let rules = parse_rules(r);

    let mut char_counts = HashMap::new();
    for c in t.chars() {
        if let Some(v) = char_counts.get_mut(&c) {
            *v += 1;
        } else {
            char_counts.insert(c, 1);
        }
    }

    for _ in 0..40 {
        polymer = step(&rules, &polymer, &mut char_counts);
    }

    let mut c: Vec<u64> = char_counts.iter().map(|(_, x)| *x).collect();
    c.sort_by(|a, b| a.cmp(b));

    println!("{}", c.last().unwrap() - c.first().unwrap());
}

type Rules = HashMap<(char, char), char>;
type Polymer = HashMap<(char, char), u64>;

fn step(r: &Rules, p: &Polymer, char_counts: &mut HashMap<char, u64>) -> Polymer {
    let mut new_polymer = HashMap::new();

    for (pair, new_char) in r {
        if let Some(count) = p.get(pair) {
            if let Some(char) = char_counts.get_mut(new_char) {
                *char += count;
            } else {
                char_counts.insert(*new_char, *count);
            }

            if let Some(v) = new_polymer.get_mut(&(pair.0, *new_char)) {
                *v += *count;
            } else {
                new_polymer.insert((pair.0, *new_char), *count);
            }

            if let Some(v) = new_polymer.get_mut(&(*new_char, pair.1)) {
                *v += *count;
            } else {
                new_polymer.insert((*new_char, pair.1), *count);
            }
        }
    }
    new_polymer
}

fn parse_polymer(s: &str) -> Polymer {
    let mut p = HashMap::new();
    for w in s.chars().collect::<Vec<char>>().windows(2) {
        let k = (w[0], w[1]);
        if let Some(v) = p.get_mut(&k) {
            *v += 1;
        } else {
            p.insert(k, 1);
        }
    }
    p
}

fn parse_rules(s: &str) -> Rules {
    let mut h = HashMap::new();
    for line in s.lines() {
        let (k, v) = line.split_once(" -> ").unwrap();
        h.insert((k.chars().nth(0).unwrap(), k.chars().nth(1).unwrap()), v.chars().nth(0).unwrap());
    }
    h
}