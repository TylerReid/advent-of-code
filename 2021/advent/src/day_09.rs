use std::{collections::HashSet, ops::RangeBounds};

use super::input;

pub fn f() {
    let input: Vec<Vec<i32>> = input::read_parse(9, |x| {
        x.chars().map(|n| n.to_string().parse().unwrap()).collect()
    });

    let mut low_points = Vec::new();
    for (x, v) in input.iter().enumerate() {
        for (y, _) in v.iter().enumerate() {
            if is_low_point(&input, (x, y)) {
                low_points.push((x, y));
            }
        }
    }

    let mut sum = 0;
    for (x, y) in low_points {
        sum += input[x][y] + 1;
    }
    println!("part 1: {}", &sum);

    find_basins(&input);
}

fn is_low_point(v: &Vec<Vec<i32>>, (x, y): (usize, usize)) -> bool {
    let x_max = v.len();
    let y_max = v[0].len();

    let up = if x + 1 < x_max { v[x + 1][y] } else { i32::MAX };
    let down = if x != 0 { v[x - 1][y] } else { i32::MAX };
    let left = if y + 1 < y_max { v[x][y + 1] } else { i32::MAX };
    let right = if y != 0 { v[x][y - 1] } else { i32::MAX };
    let center = v[x][y];

    center < up && center < down && center < left && center < right
}

fn find_basins(input: &Vec<Vec<i32>>) {
    let mut basins: Vec<HashSet<(usize, usize)>> = Vec::new();
    for (x, v) in input.iter().enumerate() {
        for y in 0..v.len() {
            if input[x][y] == 9 {
                continue;
            }
            if !basins.iter().any(|b| b.contains(&(x, y))) {
                let mut basin = HashSet::new();
                crawl_basin(input, &mut basin, (x, y));
                basins.push(basin);
            }
        }
    }

    basins.sort_by(|a, b| b.len().cmp(&a.len()));

    println!(
        "{} {} {}",
        basins[0].len(),
        basins[1].len(),
        basins[2].len()
    );
    println!("{:?}", basins[0].len() * basins[1].len() * basins[2].len());
}

fn crawl_basin(v: &Vec<Vec<i32>>, basin: &mut HashSet<(usize, usize)>, (x, y): (usize, usize)) {
    if v[x][y] != 9 {
        basin.insert((x, y));

        if x + 1 < v.len() && !basin.contains(&(x + 1, y)) {
            crawl_basin(v, basin, (x + 1, y));
        }

        if x != 0 && !basin.contains(&(x - 1, y)) {
            crawl_basin(v, basin, (x - 1, y));
        }

        if y + 1 < v[0].len() && !basin.contains(&(x, y + 1)) {
            crawl_basin(v, basin, (x, y + 1));
        }

        if y != 0 && !basin.contains(&(x, y - 1)) {
            crawl_basin(v, basin, (x, y - 1));
        }
    }
}
