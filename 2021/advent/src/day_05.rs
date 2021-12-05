use std::collections::HashMap;
use std::cmp;

use super::input;

pub fn f() {
    let input = input::read_parse(5, parse);

    let mut point_count = HashMap::new();

    for line in &input {
        let line_points = line_points(&line);
        for point in line_points {
            if !point_count.contains_key(&point) {
                point_count.insert(point, 1);
            } else {
                point_count.insert(point, 1 + *point_count.get(&point).unwrap());
            }
        }
    }

    let mut total = 0;
    for point in &point_count {
        if *point.1 >= 2 {
            total += 1;
        }
    }

    println!("{:?}", total);
}

#[derive(Debug)]
struct Line {
    start: (u32, u32),
    end: (u32, u32),
}

fn parse(s: &str) -> Line {
    let (startstr, endstr) = s.split_once(" -> ").unwrap();
    let start = startstr
        .split_once(",")
        .map(|x| (x.0.parse().unwrap(), x.1.parse().unwrap()))
        .unwrap();
    let end = endstr
        .split_once(",")
        .map(|x| (x.0.parse().unwrap(), x.1.parse().unwrap()))
        .unwrap();
    Line {
        start: start,
        end: end,
    }
}

fn line_points(l: &Line) -> Vec<(u32, u32)> {
    let mut points = Vec::new();

    let x_equal = l.start.0 == l.end.0;
    let y_equal = l.start.1 == l.end.1;

    if !x_equal && !y_equal {
        return points;
    }

    if x_equal {
        let miny = cmp::min(l.start.1, l.end.1);
        let maxy = cmp::max(l.start.1, l.end.1);

        for p in miny..=maxy {
            points.push((l.start.0, p));
        }
    }

    if y_equal {
        let minx = cmp::min(l.start.0, l.end.0);
        let maxx = cmp::max(l.start.0, l.end.0);
        for p in minx..=maxx {
            points.push((p, l.start.1));
        }
    }

    points
}
