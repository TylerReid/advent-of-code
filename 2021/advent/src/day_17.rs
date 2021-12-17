use regex::Regex;
use std::ops::Range;

pub fn f() {
    let target = parse_target(&std::fs::read_to_string("input/17").unwrap());

    println!("{:?}", target);
    fire_ze_missles(&target);
}

fn parse_target(s: &str) -> (Range<i32>, Range<i32>) {
    let regex =
        Regex::new("target area: x=(-?[0-9]+)\\.\\.(-?[0-9]+), y=(-?[0-9]+)\\.\\.(-?[0-9]+)")
            .unwrap();
    let captures = regex.captures(s).unwrap();
    println!("{:?}", captures);

    (
        captures.get(1).unwrap().as_str().parse().unwrap()
            ..captures.get(2).unwrap().as_str().parse().unwrap(),
        captures.get(3).unwrap().as_str().parse().unwrap()
            ..captures.get(4).unwrap().as_str().parse().unwrap(),
    )
}

fn step((mut dx, mut dy): (i32, i32), (x, y): (i32, i32)) -> ((i32, i32), (i32, i32)) {
    let new_position = (x + dx, y + dy);
    dx += if dx == 0 {
        0
    } else if dx > 0 {
        -1
    } else {
        1
    };
    dy -= 1;
    ((dx, dy), new_position)
}

fn fire_ze_missles(target: &(Range<i32>, Range<i32>)) {
    let mut max_y = 0;
    for x in 0..100 {
        for y in 0..10000 {
            if let Some(max) = fire(&target, (x, y)) {
                if max_y < max {
                    max_y = max;
                }
            }
        }
    }
    println!("{}", max_y);
}

fn fire(target: &(Range<i32>, Range<i32>), (mut dx, mut dy): (i32, i32)) -> Option<i32> {
    let mut position = (0, 0);
    let mut max_y = 0;
    for _ in 0..1000 {
        let result = step((dx, dy), position);
        position = result.1;
        dx = result.0.0;
        dy = result.0.1;
        if position.1 > max_y {
            max_y = position.1;
        }
        if target.0.contains(&position.0) && target.1.contains(&position.1) {
            println!("found hit with {:?} and max {}", position, max_y);
            return Some(max_y);
        }
    }
    None
}
