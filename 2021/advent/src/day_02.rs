use super::input;

pub fn f() {
    let input = input::read_parse(2, parse_input);

    let mut horizontal = 0;
    let mut depth = 0;
    let mut aim = 0;

    for action in input {
        match action {
            Action::Forward(x) => {
                horizontal += x;
                depth += aim * x;
            },
            Action::Down(x) => aim += x,
            Action::Up(x) => aim -= x,
        }
    }

    println!("{}", horizontal * depth);
}

fn parse_input(s: &str) -> Action {
    let parts: Vec<&str> = s.split(' ').collect();

    match parts[0] {
        "forward" => Action::Forward(parts[1].parse().unwrap()),
        "down" => Action::Down(parts[1].parse().unwrap()),
        "up" => Action::Up(parts[1].parse().unwrap()),
        _ => panic!("unexpected value in {}", s),
    }
}

#[derive(Debug)]
enum Action {
    Forward(i32),
    Down(i32),
    Up(i32),
}