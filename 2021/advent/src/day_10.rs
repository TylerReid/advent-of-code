pub fn f() {
    let input = std::fs::read_to_string("input/10").unwrap();

    let p1: u64 = input
        .lines()
        .map(|x| match parse(x) {
            ParseResult::Corrupt(x) => x,
            _ => 0,
        })
        .sum();

    println!("part 1 {}", p1);

    let mut p2: Vec<u64> = input
        .lines()
        .map(|x| match parse(x) {
            ParseResult::Incomplete(x) => x,
            _ => 0,
        })
        .filter(|x| *x != 0)
        .collect();

    p2.sort();
    println!("{:#?}", p2[p2.len() / 2]);
}

enum ParseResult {
    Corrupt(u64),
    Incomplete(u64),
}

fn parse(s: &str) -> ParseResult {
    let mut stack = Vec::new();

    for c in s.chars() {
        match c {
            '(' | '[' | '{' | '<' => stack.push(c),
            _ => {
                if let Some(last) = stack.pop() {
                    if !matches(last, c) {
                        return ParseResult::Corrupt(error_score(&c));
                    }
                } else {
                    return ParseResult::Corrupt(error_score(&c));
                }
            }
        }
    }

    let mut score = 0;
    for c in stack.iter().rev() {
        score *= 5;
        score += completion_score(&c);
    }
    ParseResult::Incomplete(score)
}

fn error_score(c: &char) -> u64 {
    match c {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => panic!("invalid char {}", c),
    }
}

fn completion_score(c: &char) -> u64 {
    match c {
        '(' => 1,
        '[' => 2,
        '{' => 3,
        '<' => 4,
        _ => panic!("invalid char {}", c),
    }
}

fn matches(a: char, b: char) -> bool {
    a == '(' && b == ')' || a == '[' && b == ']' || a == '{' && b == '}' || a == '<' && b == '>'
}
