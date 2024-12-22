import random
C = 0
D = 0
S = 0
T = 0
while (C < 3) and (D<3):
    T = T+1
    N1 = random.randint(1,6)
    N2 = random.randint(1,6)
    print(f'{N1} -- {N2}')

    S =S + N1 + N2
    if (N1 == 6) or (N2 ==6):
        C =C+1
    if N1 == N2:
        D=D +1
A = S // (T*2)

print(f'{C} -- {D} -- {A}')

