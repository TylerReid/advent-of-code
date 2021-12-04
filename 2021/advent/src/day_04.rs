use std::ops::Index;

pub fn f() {
    let input = std::fs::read_to_string("input/4").unwrap();

    let mut sections = input.split("\n\n").collect::<Vec<&str>>();

    let drawn_numbers = sections[0]
        .split(",")
        .map(|x| x.parse().unwrap())
        .collect::<Vec<u32>>();

    sections.remove(0);

    println!("drawn numbers: {:?}", drawn_numbers);

    for &s in &sections {
        println!("section: \n{}", s);
    }

    let mut cards = Vec::<BingoCard>::new();

    for &section in &sections {
        let lines = section.lines().collect::<Vec<&str>>();
        let mut card = [[BingoCell::Open(0); 5]; 5];
        for i in 0..5 {
            card[i] = lines[i]
                .split_whitespace()
                .map(|x| BingoCell::Open(x.parse().unwrap()))
                .collect::<Vec<BingoCell>>()
                .try_into()
                .unwrap();
        }
        cards.push(card);
    }

    let mut winners = std::collections::HashSet::<usize>::new();
    let num_cards = cards.len();
    for draw in drawn_numbers {
        let mut dumb_index = 0;
        for card in &mut cards {
            for i in 0..5 {
                for j in 0..5 {
                    card[i][j] = card[i][j].mark_if_match(draw);
                }
            }
            if has_bingo(card) {
                println!("Bingo!\n{:?}", card);
                winners.insert(dumb_index);

                if winners.len() == num_cards {
                    let sum = sum_unmarked(card);
                    println!("let the wookie win");
                    println!("{} * {} = {}", sum, draw, sum*draw);
                    return
                }
            }
            dumb_index += 1;
        }
    }
}

fn has_bingo(card: &BingoCard) -> bool {
    for i in 0..5 {
        for j in 0..5 {
            if !card[i][j].is_marked() {
                break
            }
            if j == 4 { return true }
        }

        for j in 0..5 {
            if !card[j][i].is_marked() {
                break
            }
            if j == 4 { return true }
        }
    }
    false
}

fn sum_unmarked(card: &BingoCard) -> u32 {
    let mut sum = 0;

    for row in card {
        for cell in row {
            match cell {
                BingoCell::Open(x) => sum += *x,
                BingoCell::Marked(_) => (),
            }
        }
    }

    sum
}

type BingoCard = [[BingoCell; 5]; 5];

#[derive(Debug, Copy, Clone, PartialEq, Eq, Hash)]
enum BingoCell {
    Open(u32),
    Marked(u32),
}

impl BingoCell {
    fn mark_if_match(self, n: u32) -> BingoCell {
        match self {
            BingoCell::Open(x) => {
                if x == n {
                    BingoCell::Marked(n)
                } else {
                    self
                }
            },
            BingoCell::Marked(_) => self,
        }
    }

    fn is_marked(&self) -> bool {
        match self {
            BingoCell::Open(_) => false,
            BingoCell::Marked(_) => true,
        }
    }
}
