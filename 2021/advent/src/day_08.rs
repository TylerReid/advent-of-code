use super::input;

pub fn f() {
    let values = input::read_parse(8, parse);

    let count = values.iter()
        .flat_map(|(_, out)| out)
        .filter(|x| is_unique_digit(x))
        .count();

    println!("{}", count);
}

fn parse(s: &str) -> (Vec<String>, Vec<String>) {
    let (input, output) = s.split_once(" | ").unwrap();
    (input.split_whitespace().map(|x| x.to_owned()).collect(), output.split_whitespace().map(|x| x.to_owned()).collect())
}

fn is_unique_digit(s: &str) -> bool {
    match s.len() {
        2 => true,
        4 => true,
        3 => true,
        7 => true,
        _ => false,
    }
}