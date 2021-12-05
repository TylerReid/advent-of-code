use std::cmp;
use std::collections::HashMap;

use super::input;

pub fn f() {
    let input = input::read_parse(5, parse);

    let mut point_count = HashMap::new();

    for line in &input {
        let line_points = line_points(&line);
        for point in line_points {
            let p = point_count.entry(point).or_insert(0);
            *p += 1;
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
    start: (i32, i32),
    end: (i32, i32),
}

fn parse(s: &str) -> Line {
    let (startstr, endstr) = s.split_once(" -> ").unwrap();
    let start = startstr
        .split_once(",")
        .map(|(x, y)| (x.parse().unwrap(), y.parse().unwrap()))
        .unwrap();
    let end = endstr
        .split_once(",")
        .map(|(x, y)| (x.parse().unwrap(), y.parse().unwrap()))
        .unwrap();
    Line {
        start: start,
        end: end,
    }
}

fn line_points(l: &Line) -> Vec<(i32, i32)> {
    let mut points = Vec::new();
    let start = l.start;
    let end = l.end;

    let dx: i32 = if start.0 < end.0 {
        1
    } else if start.0 > end.0 {
        -1
    } else {
        0
    };

    let dy: i32 = if start.1 < end.1 {
        1
    } else if start.1 > end.1 {
        -1
    } else {
        0
    };

    let mut current = start;
    while current != end {
        points.push(current);
        current.0 += dx;
        current.1 += dy;
    }
    points.push(end);

    points
}
