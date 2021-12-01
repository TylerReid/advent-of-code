pub fn f() {
    let input = std::fs::read_to_string("input/1")
        .expect("problem reading file")
        .lines()
        .map(|x| x.parse::<i32>().unwrap())
        .collect::<Vec<i32>>();

    part_one(&input);
    part_two(&input);
}

fn part_one(input: &Vec<i32>) {
    let mut increase_count = 0;
    for i in 1..input.len() {
        if input[i] > input[i-1] {
            increase_count += 1;
        }
    }
    println!("{}", increase_count);
}

fn part_two(input: &Vec<i32>) { 
    let mut increase_count = 0;
    for i in 3..input.len() {
        let previous = input[i-1] + input[i-2] + input[i-3];
        let current = input[i] + input[i-1] + input[i-2];
        if current > previous {
            increase_count += 1;
        }
    }
    println!("{}", increase_count);
}