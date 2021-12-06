pub fn f() {
    let input: Vec<u8> = std::fs::read_to_string("input/6")
        .unwrap()
        .split(",")
        .map(|x| x.parse::<u8>().unwrap())
        .collect();

    let mut buckets: [usize; 9] = [
        0,
        input.iter().filter(|x| **x == 1).count(),
        input.iter().filter(|x| **x == 2).count(),
        input.iter().filter(|x| **x == 3).count(),
        input.iter().filter(|x| **x == 4).count(),
        input.iter().filter(|x| **x == 5).count(),
        input.iter().filter(|x| **x == 6).count(),
        0,
        0,
    ];

    for _ in 0..256 {
        tick(&mut buckets);
    }
    println!("{}", buckets.iter().sum::<usize>())
}

fn tick(buckets: &mut [usize; 9]) {
    let ready_to_spawn = buckets[0];

    for i in 0..8 {
        buckets[i] = buckets[i+1];
    }

    buckets[6] += ready_to_spawn;
    buckets[8] = ready_to_spawn;
}