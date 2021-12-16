extern crate hex;
use bitvec::prelude::*;
use std::rc::Rc;

pub fn f() {
    let input = std::fs::read_to_string("input/16").unwrap();

    let bytes = hex::decode(input).unwrap();
    let bits = BitVec::<Msb0, _>::from_vec(bytes);

    let (packet, _) = parse_packet(&bits);
    println!("{:?}", packet);

    println!("sum: {}", sum_version_numbers(&packet));
}

#[derive(Debug)]
enum Value {
    Literal(u64),
    InnerValue(Vec<Packet>),
}

#[derive(Debug)]
struct Packet {
    version: u64,
    value: Value,
}

fn parse_packet(bits: &BitSlice<Msb0, u8>) -> (Packet, usize) {
    println!("\n\nbits: {}", bits);
    let mut bits_read = 0;
    let version = slice_to_int(&bits[bits_read..3]);
    bits_read += 3;
    let type_id = slice_to_int(&bits[bits_read..bits_read+3]);
    bits_read += 3;
    println!("version {} type {}", version, type_id);

    let value = match type_id {
        4 => {
            let (v, b) = slice_to_literal(&bits[bits_read..bits.len()]);
            bits_read += b;
            Value::Literal(v)
        },
        _ => {
            let length_type_id = bits[bits_read];
            bits_read += 1;
            println!("length id: {}", length_type_id);
            println!("remaining: {}", &bits[bits_read..bits.len()]);
            let value = if length_type_id == false {
                let subpacket_length = slice_to_int(&bits[bits_read..bits_read+15]);
                bits_read += 15;
                println!("subpacket length: {}", subpacket_length);
                let mut subpackets = Vec::new();
                let read_until = bits_read + subpacket_length as usize;
                while bits_read < read_until {
                    let (subpacket, r) = parse_packet(&bits[bits_read..bits.len()]);
                    bits_read += r;
                    subpackets.push(subpacket);
                }
                Value::InnerValue(subpackets)
            } else {
                println!("num packet bits {}", &bits[bits_read..bits_read+11]);
                let num_subpackets = slice_to_int(&bits[bits_read..bits_read+11]);
                bits_read += 11;
                println!("num subpackets {}", num_subpackets);
                let mut subpackets = Vec::new();
                for _ in 0..num_subpackets {
                    let (subpacket, r) = parse_packet(&bits[bits_read..bits.len()]);
                    bits_read += r;
                    subpackets.push(subpacket);
                }
                Value::InnerValue(subpackets)
            };

            value
        }
    };

    (
        Packet {
            version: version,
            value: value,
        },
        bits_read,
    )
}

// this seems silly, I am probably missing something in bitvec to do this
fn slice_to_int(slice: &BitSlice<Msb0, u8>) -> u64 {
    //todo make this generic on int type?
    let mut result = 0;

    for bit in slice.to_bitvec() {
        result <<= 1;
        result |= if bit { 1 } else { 0 }
    }

    result
}

fn slice_to_literal(slice: &BitSlice<Msb0, u8>) -> (u64, usize) {
    let mut result = 0;
    let mut bits_read = 0;

    for chunk in slice.chunks(5) {
        let mut exit = false;
        for (i, b) in chunk.iter().enumerate() {
            bits_read += 1;
            if i == 0 {
                if b == false {
                    exit = true;
                }
                continue;
            }
            result <<= 1;
            result |= if b == true { 1 } else { 0 };
        }
        if exit {
            return (result, bits_read);
        }
    }
    (result, bits_read)
}

fn sum_version_numbers(packet: &Packet) -> u64 {
    let mut result = packet.version;
    if let Value::InnerValue(sub_packets) = &packet.value {
        for sub_packet in sub_packets {
            result += sum_version_numbers(sub_packet);
        }
    }
    result
}
