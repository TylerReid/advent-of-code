use super::input;

pub fn f() {
    let input = input::read(2);

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

impl std::str::FromStr for Action {
    type Err = std::num::ParseIntError;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let (d, v) = s.split_once(' ').unwrap();
        let value = v.parse()?;
        let result = match d {
            "forward" => Action::Forward(value),
            "down" => Action::Down(value),
            "up" => Action::Up(value),
            _ => panic!("unexpected value in {}", s),
        };
        Ok(result)
    }
}

#[derive(Debug)]
enum Action {
    Forward(i32),
    Down(i32),
    Up(i32),
}