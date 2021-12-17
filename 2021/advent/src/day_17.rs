use regex::Regex;
use std::ops::Range;

pub fn f() {
    let target = parse_target(&std::fs::read_to_string("input/17").unwrap());

    println!("{:?}", target);
    println!("{:?}", fire(&target, (6, 4)));

    fire_ze_missles(&target);
}

fn parse_target(s: &str) -> (Range<i32>, Range<i32>) {
    let regex =
        Regex::new("target area: x=(-?[0-9]+)\\.\\.(-?[0-9]+), y=(-?[0-9]+)\\.\\.(-?[0-9]+)")
            .unwrap();
    let captures = regex.captures(s).unwrap();
    println!("{:?}", captures);

    let x_start = captures.get(1).unwrap().as_str().parse().unwrap();
    let x_end = captures.get(2).unwrap().as_str().parse::<i32>().unwrap() + 1;
    let y_start = captures.get(3).unwrap().as_str().parse().unwrap();
    let y_end = captures.get(4).unwrap().as_str().parse::<i32>().unwrap() + 1;

    (x_start..x_end, y_start..y_end)
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
    let mut hit_count = 0;
    for x in -1000..1000 {
        for y in -1000..1000 {
            if let Some(max) = fire(&target, (x, y)) {
                hit_count += 1;
                if max_y < max {
                    max_y = max;
                }
            }
        }
    }
    println!("max y {}, hit count {}", max_y, hit_count);
}

fn fire(target: &(Range<i32>, Range<i32>), (mut dx, mut dy): (i32, i32)) -> Option<i32> {
    let mut position = (0, 0);
    let mut max_y = 0;
    let og_velocity = (dx, dy);
    for _ in 0..1000 {
        let result = step((dx, dy), position);
        position = result.1;
        dx = result.0 .0;
        dy = result.0 .1;

        if target.0.end < position.0 || target.1.start > position.1 {
            return None
        }

        if position.1 > max_y {
            max_y = position.1;
        }
        if target.0.contains(&position.0) && target.1.contains(&position.1) {
            println!("found hit in {:?} with velocity {:?} and max {}", position, og_velocity, max_y);
            return Some(max_y);
        }
    }
    None
}
