use std::{collections::HashSet, hash::Hash};

pub fn f() {
    let input = std::fs::read_to_string("input/13").unwrap();

    let (p, f) = input.split_once("\n\n").unwrap();
    let points = parse_points(p);
    let folds = parse_folds(f);

    let n = fold(points, &folds[0]);
    println!("{}", n.len());
}

fn fold(h: HashSet<(u16, u16)>, fold: &Fold) -> HashSet<(u16, u16)> {
    let mut new_set = HashSet::new();
    for (x, y) in h {
        match fold {
            Fold::X(f) => {
                if x > *f {
                    new_set.insert((f - (x - f), y));
                } else {
                    new_set.insert((x, y));
                }
            }
            Fold::Y(f) => {
                if y > *f {
                    new_set.insert((x, f - (y - f)));
                } else {
                    new_set.insert((x, y));
                }
            }
        }
    }
    new_set
}

fn parse_points(s: &str) -> HashSet<(u16, u16)> {
    let mut points = HashSet::new();
    for line in s.lines() {
        let (x, y) = line.split_once(",").unwrap();
        points.insert((x.parse().unwrap(), y.parse().unwrap()));
    }
    points
}

fn parse_folds(s: &str) -> Vec<Fold> {
    let mut folds = Vec::new();
    for line in s.lines() {
        let (start, end) = line.split_once("=").unwrap();
        if start.ends_with("y") {
            folds.push(Fold::Y(end.parse().unwrap()));
        } else {
            folds.push(Fold::X(end.parse().unwrap()));
        }
    }

    folds
}

#[derive(Debug)]
enum Fold {
    X(u16),
    Y(u16),
}
