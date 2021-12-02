use std::fmt;
use std::str::FromStr;

pub fn read<T>(day: u8) -> Vec<T> where T: FromStr, <T as FromStr>::Err: fmt::Debug {
    std::fs::read_to_string(format!("input/{}", day))
        .expect("problem reading file")
        .lines()
        .map(|x| x.parse::<T>().unwrap())
        .collect::<Vec<T>>()
}

pub fn read_parse<T>(day: u8, parse: fn(&str) -> T) -> Vec<T> {
    std::fs::read_to_string(format!("input/{}", day))
        .expect("problem reading file")
        .lines()
        .map(|x| parse(x))
        .collect::<Vec<T>>()
} 