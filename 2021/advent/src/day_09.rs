use super::input;

pub fn f() {
    let input: Vec<Vec<i32>> = input::read_parse(9, |x| x.chars().map(|n| n.to_string().parse().unwrap()).collect());

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
    println!("{}", &sum);
}


fn is_low_point(v: &Vec<Vec<i32>>, (x, y): (usize, usize)) -> bool {
    let x_max = v.len();
    let y_max = v[0].len();

    let up = if x + 1 < x_max { v[x+1][y] } else { i32::MAX };
    let down = if x != 0 { v[x-1][y] } else { i32::MAX };
    let left = if y + 1 < y_max { v[x][y+1] } else { i32::MAX };
    let right = if y != 0 { v[x][y-1] } else { i32::MAX };
    let center = v[x][y];

    center < up && center < down && center < left && center < right
}
