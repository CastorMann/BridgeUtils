###################################################################################################
######  PARSE A DESCRIPTION OF A BIDDING SEQUENCE AGAINST A HAND AND RETURN TRUE OF FALSE  ########
###################################################################################################

import sys

def get_hcp(hand):
    res = 0
    cards = list(map(int, hand.split(".")))
    for card in cards:
        res += max(0, (card % 13) - 8)
    return res

def get_spades(hand):
    res = 0
    cards = list(map(int, hand.split(".")))
    for card in cards:
        if 39 <= card < 52 : res += 1
    return res

def get_hearts(hand):
    res = 0
    cards = list(map(int, hand.split(".")))
    for card in cards:
        if 26 <= card < 39 : res += 1
    return res

def get_diamonds(hand):
    res = 0
    cards = list(map(int, hand.split(".")))
    for card in cards:
        if 13 <= card < 26 : res += 1
    return res

def get_clubs(hand):
    res = 0
    cards = list(map(int, hand.split(".")))
    for card in cards:
        if 0 <= card < 13 : res += 1
    return res

def get_balanced(hand):
    s = get_spades(hand)
    h = get_hearts(hand)
    d = get_diamonds(hand);
    c = get_clubs(hand);
    if min([s, h, d, c]) < 2: return False
    if max([s, h, d, c]) > 5: return False
    if [s, h, d, c].count(2) == 2: return False
    return True

def get_semibalanced(hand):
    s = get_spades(hand)
    h = get_hearts(hand)
    d = get_diamonds(hand);
    c = get_clubs(hand);
    if min([s, h, d, c]) < 2: return False
    if max([s, h, d, c]) > 5: return False
    return True

class Hand:
    def __init__(self, hand):
        self.hcp = get_hcp(hand)
        self.spades = get_spades(hand)
        self.hearts = get_hearts(hand)
        self.diamonds = get_diamonds(hand)
        self.clubs = get_clubs(hand)
        self.balanced = get_balanced(hand)
        self.semibalanced = get_semibalanced(hand)
        # TODO: add self.losers, self.controls, etc


def parse(hand, constraints):
    hand = Hand(hand)
    spades = hand.spades
    hearts = hand.hearts
    diamonds = hand.diamonds
    clubs = hand.clubs
    hcp = hand.hcp
    balanced = hand.balanced
    semibalanced = hand.semibalanced

    S = spades
    H = hearts
    D = diamonds
    C = clubs
    return eval(constraints)


HAND = "4.5.6.7.9.12.16.22.24.25.44.48.49"

CONSTRAINTS_1 = "clubs > 8 and hcp > 9 and (spades > 3 or diamonds < 5)"
CONSTRAINTS_2 = "spades > 1"
CONSTRAINTS_3 = "hcp > 10"

if __name__ == "__main__":
    print(parse(sys.argv[1], sys.argv[2]))