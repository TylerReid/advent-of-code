use super::input;

pub fn f() {
    let input = input::read_parse(3, |x| i32::from_str_radix(x, 2).unwrap());
    let mut bit_counts = [0; 12];

    let mut mask = 1;
    let mut gamma = 0;
    let mut epsilon = 0;
    for i in 0..12 {
        for x in &input {
            bit_counts[i] += if x & mask == 0 { 0 } else { 1 };
        }

        if bit_counts[i] > input.len() / 2 {
            gamma = gamma | mask;
        } else {
            epsilon = epsilon | mask;
        }

        mask = mask << 1;
    }

    println!("{:?}", gamma * epsilon);
}

const BITLEN: usize = 12;
pub fn part2() {
    let input = input::read_parse(3, |x| i32::from_str_radix(x, 2).unwrap());
    let oxygen = part2_filter(&input, true);
    let c02 = part2_filter(&input, false);

    println!("{}", oxygen * c02);
}

fn part2_filter(input: &Vec<i32>, is_oxygen: bool) -> i32 {
    let mut bit_counts = [0; BITLEN];
    let mut mask = 1 << (BITLEN - 1);

    let mut list = input.clone();

    for i in (0..BITLEN).rev() {
        for x in &list {
            bit_counts[i] += if x & mask == 0 { 0 } else { 1 };
        }

        let num_set = bit_counts[i];
        let num_not_set = list.len() - num_set;
        list.retain(|x| {
            if num_set >= num_not_set {
                (x & mask != 0) == is_oxygen
            } else {
                (x & mask == 0) == is_oxygen
            }
        });

        if list.len() == 1 {
            return list[0]
        }

        mask = mask >> 1;
    }

    0
}
