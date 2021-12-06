pub fn f() {
    let input: Vec<usize> = std::fs::read_to_string("input/6")
        .unwrap()
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect();

    let mut buckets = [0; 9];
    for f in input {
        buckets[f] += 1;
    }

    for _ in 0..256 {
        buckets.rotate_left(1);
        buckets[6] += buckets[8];
    }
    println!("{}", buckets.iter().sum::<usize>())
}
