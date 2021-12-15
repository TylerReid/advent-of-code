use super::input;

pub fn f() {
    let input: Vec<Vec<u32>> = input::read_parse(15, |s| {
        s.chars().map(|c| c.to_string().parse().unwrap()).collect()
    });

    let costs = find_path(&input, 0, (0, 0));

    println!("{:?}", costs.iter().min().unwrap());
}

fn find_path(cave: &Vec<Vec<u32>>, current_cost: u32, (x, y): (usize, usize)) -> Vec<u32> {
    let x_max = cave.len() - 1;
    let y_max = cave[0].len() - 1;

    if x == x_max && y == y_max {
        return vec![current_cost];
    }

    if x == x_max {
        return find_path(cave, current_cost + cave[x][y + 1], (x, y + 1));
    }

    if y == y_max {
        return find_path(cave, current_cost + cave[x + 1][y], (x + 1, y));
    }

    let mut costs = find_path(cave, current_cost + cave[x][y + 1], (x, y + 1));
    costs.append(&mut find_path(
        cave,
        current_cost + cave[x + 1][y],
        (x + 1, y),
    ));

    costs
}
