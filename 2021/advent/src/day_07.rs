pub fn f() {
    let input: Vec<i32> = std::fs::read_to_string("input/7")
        .unwrap()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    dumb_n2_solution(&input);
}


fn dumb_n2_solution(positions: &[i32]) {
    let mut lowest = (0, i32::MAX);

    let min = *positions.iter().min().unwrap();
    let max = *positions.iter().max().unwrap();

    for position in min..=max {
        let mut cost = 0;
        for other in positions {
            cost += trangle_number((position - other).abs());
        }
        if cost < lowest.1 {
            lowest = (position, cost);
        }
    }

    println!("{:?}", lowest);
}

// I think there is a way to do this without the loop
// because this forms a trangle with area (n^2 / 2) + n/2
// but I am derping on the integer math needed for this
fn calc_cost(n: i32) -> i32 {
    (0..=n).sum()
}

// :facepalm: copied from sajattack's solution 
// when I tried this on my own I must have derped on typing it in and confused myself
fn trangle_number(num: i32) -> i32 {
    (num.pow(2) + num)/2
}