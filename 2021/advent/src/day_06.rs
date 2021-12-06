pub fn f() {
    let mut fishies: Vec<Fish> = std::fs::read_to_string("input/6")
        .unwrap()
        .split(",")
        .map(|x| x.parse::<Fish>().unwrap())
        .collect();

    for _ in 0..80 {
        let mut new_fish = Vec::<Fish>::new();
        for fish in &mut fishies {
            if let Some(new) = tick(fish) {
                new_fish.push(new);
            }
        }
        fishies.append(&mut new_fish);
    }
    println!("{}", fishies.len());
}

type Fish = i16;

fn tick(fish: &mut Fish) -> Option<Fish> {
    if *fish == 0 {
        *fish = 6;
        return Some(8);
    }
    *fish -= 1;
    None
}