use super::input;

pub fn f() {
    let input = input::read_parse(18, parse);

    let mut max_mag = 0;
    for a in 0..input.len() {
        for b in 0..input.len() {
            if a == b {
                continue;
            }
            let mut number = add_numbers(&input[a], &input[b]);
            while let Some(n) = simplify(&number) {
                number = n;
            }
            while number.len() > 1 {
                calc_magnitude(&mut number);
            }
            max_mag = max_mag.max(number[0].value);
        }
    }

    println!("{}", max_mag);
}

#[derive(Debug, Clone, Copy)]
struct Number {
    value: u64,
    level: u8,
}

fn parse(s: &str) -> Vec<Number> {
    let mut level = 0;
    let mut numbers = Vec::new();
    for c in s.chars() {
        match c {
            '[' => level += 1,
            ']' => level -= 1,
            ',' => (),
            n => {
                numbers.push(Number {
                    value: n.to_string().parse().unwrap(),
                    level,
                });
            }
        }
    }
    numbers
}

fn print_numbers(v: &[Number]) {
    let mut level = 0;
    for n in v {
        while level < n.level {
            print!("[");
            level += 1;
        }
        while level > n.level {
            print!("]");
            level -= 1;
        }
        print!(",{}", n.value);
    }
    println!("]");
}

fn add_numbers(a: &[Number], b: &[Number]) -> Vec<Number> {
    let mut result = Vec::new();
    for n in a {
        result.push(Number {
            value: n.value,
            level: n.level + 1,
        });
    }
    for n in b {
        result.push(Number {
            value: n.value,
            level: n.level + 1,
        });
    }
    result
}

fn simplify(number: &Vec<Number>) -> Option<Vec<Number>> {
    for (i, n) in number.iter().enumerate() {
        if n.level > 4 && i + 1 < number.len() && n.level == number[i + 1].level {
            return Some(explode(number, i));
        }
    }

    for (i, n) in number.iter().enumerate() {
        if n.value > 9 {
            return Some(split(number, i));
        }
    }
    None
}

fn explode(number: &Vec<Number>, index: usize) -> Vec<Number> {
    let mut result = number.clone();

    let left_val = result[index].value;
    let right_val = result[index + 1].value;

    if index > 0 {
        result[index - 1].value += left_val;
    }

    if let Some(v) = result.get_mut(index+2) {
        v.value += right_val;
    }

    result.remove(index);
    result.remove(index);
    result.insert(
        index,
        Number {
            value: 0,
            level: number[index].level - 1,
        },
    );

    //print!("after explode:  ");
    result
}

fn split(number: &Vec<Number>, index: usize) -> Vec<Number> {
    let mut result = number.clone();

    result.remove(index);
    result.insert(
        index,
        Number {
            value: div_ceil(number[index].value),
            level: number[index].level + 1,
        },
    );
    result.insert(
        index,
        Number {
            value: div_floor(number[index].value),
            level: number[index].level + 1,
        },
    );
    //print!("after split:    ");
    result
}

const fn div_floor(n: u64) -> u64 {
    n / 2
}

const fn div_ceil(n: u64) -> u64 {
    let d = n / 2;
    let r = n % 2;
    if r > 0 {
        d + 1
    } else {
        d
    }
}

fn calc_magnitude(number: &mut Vec<Number>) {
    for i in 0..number.len() - 1 {
        let left = number[i];
        let right = number[i + 1];
        if left.level == right.level {
            let a = left.value * 3;
            let b = right.value * 2;
            number.remove(i);
            number.remove(i);
            number.insert(
                i,
                Number {
                    level: left.level - 1,
                    value: a + b,
                },
            );
            break;
        }
    }
}
