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
    derp(caves, "start", "start", false).unwrap()
}

fn derp(caves: &HashMap<&str, Vec<&str>>, past_path: &str, current_position: &str, double_dipped: bool) -> Option<Vec<String>> {
    if current_position == "end" {
        return Some(vec![past_path.to_string()]);
    }
    
    let next_moves = &caves[current_position];
    let mut paths = Vec::new();
    for m in next_moves {
        if m == &"start" {
            continue;
        }
        let mut dd = double_dipped;
        if m.chars().all(|x| x.is_lowercase()) && past_path.contains(*m) {
            if double_dipped {
                continue;
            }
            dd = true;
        }

        if let Some(p) = derp(caves, &format!("{},{}", past_path, m), m, dd).as_mut() {
            paths.append(p);
        }
    }

    if paths.len() > 0 {
        return Some(paths);
    }

    None
}