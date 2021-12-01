use std::fmt;
use std::str::FromStr;

pub fn read<T>(name: &str) -> Vec<T> where T: FromStr, <T as FromStr>::Err: fmt::Debug {
    std::fs::read_to_string("input/".to_owned() + name)
        .expect("problem reading file")
        .lines()
        .map(|x| x.parse::<T>().unwrap())
        .collect::<Vec<T>>()
}