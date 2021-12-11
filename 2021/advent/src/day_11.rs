use super::input;

pub fn f() {
    let mut octos = input::read_parse(11, parse);

    for i in 1..i32::MAX {
        let num_flashes = step(&mut octos);

        if num_flashes == 100 {
            println!("{}", i);
            return;
        }
    }
}

fn step(octos: &mut Vec<Vec<u16>>) -> u32 {
    for x  in 0..10 {
        for y in 0..10 {
            add_energy(octos, x, y)
        }
    }

    clear_board(octos)
}

fn add_energy(octos: &mut Vec<Vec<u16>>, x: usize, y: usize) {
    let o = &mut octos[x][y];
    *o += 1;
    let valid_index = 0..10;
    if *o == 10 {
        for i in -1..=1 {
            for j in -1..=1 {
                let new_x = x as i32 + i;
                let new_y = y as i32 + j;
                if valid_index.contains(&new_x) && valid_index.contains(&new_y) {
                    add_energy(octos, new_x.try_into().unwrap(), new_y.try_into().unwrap());
                }
            }
        }
    }
}

fn clear_board(octos: &mut Vec<Vec<u16>>) -> u32 {
    let mut flashes = 0;
    for x in 0..10 {
        for y in 0..10 {
            let o = &mut octos[x][y];
            if *o > 9 {
                *o = 0;
                flashes += 1;
            }
        }
    }
    flashes
}

fn parse(s: &str) -> Vec<u16> {
    s.chars().map(|x| x.to_string().parse().unwrap()).collect()
}