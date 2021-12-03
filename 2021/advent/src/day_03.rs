use super::input;

pub fn f() {
    let input = input::read_parse(3, |x| i32::from_str_radix(x, 2).unwrap());
    let mut bit_counts = [0;12];

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
