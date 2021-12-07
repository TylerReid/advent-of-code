pub fn f() {
    let input: Vec<i32> = std::fs::read_to_string("input/7")
        .unwrap()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    dumb_n2_solution(&input);
}


fn dumb_n2_solution(positions: &Vec<i32>) {
    let mut lowest = (0, i32::MAX);

    for (i, position) in positions.iter().enumerate() {
        let mut cost = 0;
        for (j, other) in positions.iter().enumerate() {
            if i == j {
                continue;
            }
            cost += (position - other).abs();
        }
        if cost < lowest.1 {
            lowest = (*position, cost);
        }
    }

    println!("{:?}", lowest);
}