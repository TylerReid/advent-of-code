use std::{collections::HashMap, vec};

use super::input;

pub fn f() {
    let values = input::read_parse(8, parse);

    let count = values
        .iter()
        .flat_map(|(_, out)| out)
        .filter(|x| {
            if let Some(_) = unique_digit(x) {
                true
            } else {
                false
            }
        })
        .count();

    println!("{}", count);

    part_two(&values);
}

fn part_two(input: &[(Vec<String>, Vec<String>)]) {
    let sum: u32 = input.iter().map(|x| deduce_output(x)).sum();
    println!("{}", sum);
}

fn deduce_output((input, output): &(Vec<String>, Vec<String>)) -> u32 {
    let mut values = HashMap::new();

    values.insert(get_unique(input, 1), 1);
    values.insert(get_unique(input, 4), 4);
    values.insert(get_unique(input, 7), 7);
    values.insert(get_unique(input, 8), 8);

    while values.len() != 10 {
        for i in input {
            if values.contains_key(i) {
                continue;
            }
            match i.len() {
                5 => {
                    if overlap_count(i, find_key(&values, 1)) == 2 {
                        values.insert(i.clone(), 3);
                    } else if let Some(six) = find_key(&values, 6) {
                        if overlap_count(&six, Some(i.clone())) == 5 {
                            values.insert(i.clone(), 5);
                        } else {
                            values.insert(i.clone(), 2);
                        }
                    }
                }
                6 => {
                    if overlap_count(i, find_key(&values, 1)) != 2 {
                        values.insert(i.clone(), 6);
                    } else if let Some(five) = find_key(&values, 5) {
                        if overlap_count(&five, Some(i.clone())) == 5 {
                            values.insert(i.clone(), 9);
                        } else {
                            values.insert(i.clone(), 0);
                        }
                    }
                }
                _ => panic!("invalid length {}", i),
            }
        }
    }

    println!("output {:#?}", output);
    println!("map {:#?}", values);

    output
        .iter()
        .map(|x| values.get(x).unwrap())
        .sum::<u8>()
        .try_into()
        .unwrap()
}

fn parse(s: &str) -> (Vec<String>, Vec<String>) {
    let (input, output) = s.split_once(" | ").unwrap();
    (
        input.split_whitespace().map(|x| x.to_owned()).collect(),
        output.split_whitespace().map(|x| x.to_owned()).collect(),
    )
}

fn find_key(h: &HashMap<String, u8>, v: u8) -> Option<String> {
    h.iter()
        .find_map(|(key, value)| if *value == v { Some(key.clone()) } else { None })
}

fn unique_digit(s: &str) -> Option<u8> {
    match s.len() {
        2 => Some(1),
        4 => Some(4),
        3 => Some(7),
        7 => Some(8),
        _ => None,
    }
}

fn get_unique(input: &[String], n: u8) -> String {
    for i in input {
        match unique_digit(i) {
            Some(x) => {
                if x == n {
                    return i.clone();
                }
            }
            None => (),
        }
    }
    panic!("no unique digit {} in {:#?}", n, input);
}

fn overlap_count(a: &str, b: Option<String>) -> u8 {
    if let Some(x) = b {
        return x
            .chars()
            .filter(|c| a.contains(*c))
            .count()
            .try_into()
            .unwrap();
    }
    0
}
