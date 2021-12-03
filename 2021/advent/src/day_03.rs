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

pub fn part2() {
    const Bitlen: usize = 12;
    let input = input::read_parse(3, |x| i32::from_str_radix(x, 2).unwrap());
    let mut bit_counts = [0; Bitlen];
    let mut mask = 1 << (Bitlen - 1);
    let mut oxygen_list = input.clone();
    let mut oxygen = 0;

    for i in (0..Bitlen).rev() {
        for x in &oxygen_list {
            bit_counts[i] += if x & mask == 0 { 0 } else { 1 };
        }

        let num_set = bit_counts[i];
        let num_not_set = oxygen_list.len() - num_set;
        oxygen_list.retain(|x| {
            if num_set >= num_not_set {
                x & mask != 0
            } else {
                x & mask == 0
            }
        });

        if oxygen_list.len() == 1 {
            oxygen = oxygen_list[0];
            println!("found oxygen {}", oxygen);
            break;
        }

        mask = mask >> 1;
    }

    mask = 1 << (Bitlen - 1);
    bit_counts = [0; Bitlen];
    let mut c02_list = input.clone();
    let mut c02 = 0;
    for i in (0..Bitlen).rev() {
        for x in &c02_list {
            bit_counts[i] += if x & mask == 0 { 0 } else { 1 };
        }

        let num_set = bit_counts[i];
        let num_not_set = c02_list.len() - num_set;
        c02_list.retain(|x| {
            if num_set >= num_not_set {
                x & mask == 0
            } else {
                x & mask != 0
            }
        });

        if c02_list.len() == 1 {
            c02 = c02_list[0];
            println!("found c02 {}", c02);
            break;
        }

        mask = mask >> 1;
    }

    println!("{}", oxygen * c02);
}
