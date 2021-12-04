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

    for draw in drawn_numbers {
        for card in &mut cards {
            for i in 0..5 {
                for j in 0..5 {
                    card[i][j] = card[i][j].mark_if_match(draw);
                }
            }
        }
    }

    println!("cards:\n{:?}", cards);
}

fn has_bingo(card: BingoCard) -> bool {
    let mut previous = card[0][0];
    for i in 0..5 {
        for j in 0..5 {
            if card[i][j].equals(previous) && (i == 4 || j == 4) {
                return true
            }
        }
    }
    todo!()
}

type BingoCard = [[BingoCell; 5]; 5];

#[derive(Debug, Copy, Clone)]
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

    fn equals(&self, other: BingoCell) -> bool {
        match other {
            BingoCell::Open(_) => false,
            BingoCell::Marked(n) => match self {
                BingoCell::Open(_) => false,
                BingoCell::Marked(x) => *x == n,
            },
        }
        
    }
}
