use std::collections::HashMap;

pub fn f() {
    let input = std::fs::read_to_string("input/12").unwrap();

    let mut caves: HashMap<&str, Vec<&str>> = HashMap::new();
    for line in input.lines() {
        let (from, to) = line.split_once("-").unwrap();

        if let Some(p) = caves.get_mut(from) {
            p.push(to);
        } else {
            caves.insert(from, vec![to]);
        }

        if let Some(p) = caves.get_mut(to) {
            p.push(from);
        } else {
            caves.insert(to, vec![from]);
        }
    }

    let paths = traverse(&caves);

    println!("{:?}", paths.len());
}

fn traverse(caves: &HashMap<&str, Vec<&str>>) -> Vec<String> {
    derp(caves, "start", "start").unwrap()
}

fn derp(caves: &HashMap<&str, Vec<&str>>, past_path: &str, current_position: &str) -> Option<Vec<String>> {
    if current_position == "end" {
        // println!("found end {} {}", past_path, current_position);
        return Some(vec![past_path.to_string()]);
    }
    
    let next_moves = &caves[current_position];
    // println!("past path {} current {}", past_path, current_position);
    // println!("next {:?}", next_moves);
    let mut paths = Vec::new();
    for m in next_moves {
        if m.chars().all(|x| x.is_lowercase()) && past_path.contains(*m) {
            continue;
        }

        if let Some(p) = derp(caves, &format!("{},{}", past_path, m), m).as_mut() {
            paths.append(p);
        }
    }

    if paths.len() > 0 {
        return Some(paths);
    }

    None
}