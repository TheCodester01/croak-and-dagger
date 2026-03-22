#!/usr/bin/env python3
"""Generate Wildspire Waste desert tileset + backgrounds for Godot 2D platformer."""
from PIL import Image, ImageDraw
import os

T = 32  # tile size
COLS = 8
OUT = os.path.dirname(os.path.abspath(__file__))

# --- Palette ---
SL  = (238, 210, 132)   # sand light
SM  = (212, 168,  71)   # sand mid
SD  = (165, 118,  42)   # sand dark
SE  = (190, 148,  62)   # sand edge/shadow

RL  = (188, 168, 130)   # rock light
RM  = (140, 123,  94)   # rock mid
RD  = ( 90,  72,  56)   # rock dark
RS  = ( 58,  46,  36)   # rock shadow

DL  = (172, 102,  64)   # dirt light
DM  = (148,  72,  40)   # dirt mid
DD  = ( 95,  48,  28)   # dirt dark
DP  = ( 55,  38,  28)   # deep underground

SKT = (230, 162,  68)   # sky top
SKM = (215, 148,  88)   # sky mid
SKB = (195, 130, 108)   # sky bottom
HZC = (220, 182, 138)   # haze/dust

DN  = (178, 128,  52)   # dune near
DF  = (140,  95,  35)   # dune far
DK  = (105,  70,  25)   # dune dark

CL  = ( 95, 152,  72)   # cactus light
CM  = ( 65, 112,  50)   # cactus mid
CD  = ( 42,  82,  36)   # cactus dark

AL  = (212, 190, 152)   # ancient stone light
AM  = (168, 144, 108)   # ancient stone mid
AD  = (112,  90,  68)   # ancient stone dark

WD  = (135, 102,  62)   # wood light
WDK = ( 92,  72,  44)   # wood dark

SUN = (255, 220,  80)   # sun
SUH = (255, 240, 140)   # sun highlight
SUNR= (240, 130,  40)   # sun ray/glow

TR  = (0, 0, 0, 0)      # transparent

def mk():
    return Image.new('RGBA', (T, T), TR)

def fill(d, c):
    d.rectangle([0, 0, T-1, T-1], fill=c)

def rect(d, x1, y1, x2, y2, c):
    d.rectangle([x1, y1, x2, y2], fill=c)

def px(img, x, y, c):
    if 0 <= x < T and 0 <= y < T:
        img.putpixel((x, y), c + (255,) if len(c) == 3 else c)

def hline(img, y, x1, x2, c):
    for x in range(x1, x2+1):
        px(img, x, y, c)

def vline(img, x, y1, y2, c):
    for y in range(y1, y2+1):
        px(img, x, y, c)

def sand_texture(img, y_start=0, y_end=T-1, x_start=0, x_end=T-1):
    """Add subtle sand grain texture."""
    import random
    rng = random.Random(42)
    for y in range(y_start, y_end+1):
        for x in range(x_start, x_end+1):
            r = rng.random()
            if r < 0.08:
                px(img, x, y, SL)
            elif r < 0.14:
                px(img, x, y, SD)

def rock_texture(img, y_start=0, y_end=T-1, x_start=0, x_end=T-1):
    """Add subtle rock crack texture."""
    import random
    rng = random.Random(13)
    for y in range(y_start, y_end+1):
        for x in range(x_start, x_end+1):
            r = rng.random()
            if r < 0.06:
                px(img, x, y, RL)
            elif r < 0.10:
                px(img, x, y, RD)

# =====================================================================
# GROUND TILES
# =====================================================================

def tile_ground_sand_TC():
    """Top-center ground sand tile: sandy surface on top, solid fill below."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 6, T-1, T-1, SM)
    # top surface edge
    hline(img, 5, 0, T-1, SE)
    hline(img, 4, 0, T-1, SL)
    hline(img, 3, 0, T-1, SL)
    # highlight specks on surface
    for x in [3, 9, 17, 25, 29]:
        px(img, x, 4, SL)
    sand_texture(img, 7, T-1)
    return img

def tile_ground_sand_TL():
    """Top-left corner: sand surface, transparent upper-left."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 6, 6, T-1, T-1, SM)
    rect(d, 6, 3, T-1, 5, SE)
    rect(d, 6, 2, T-1, 4, SL)
    # left wall
    for y in range(6, T):
        px(img, 5, y, SE)
        px(img, 4, y, SL)
    # corner pixel
    px(img, 5, 5, SE)
    sand_texture(img, 7, T-1, 7, T-1)
    return img

def tile_ground_sand_TR():
    """Top-right corner: sand surface, transparent upper-right."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 6, T-7, T-1, SM)
    rect(d, 0, 3, T-7, 5, SE)
    rect(d, 0, 2, T-7, 4, SL)
    for y in range(6, T):
        px(img, T-6, y, SE)
        px(img, T-5, y, SL)
    px(img, T-6, 5, SE)
    sand_texture(img, 7, T-1, 0, T-8)
    return img

def tile_fill_sand():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-1, SM)
    sand_texture(img)
    return img

def tile_fill_rock():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-1, RM)
    rock_texture(img)
    # subtle crack lines
    for y in [8, 20]:
        hline(img, y, 4, 12, RD)
        hline(img, y, 18, 26, RD)
    return img

def tile_fill_dark():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-1, DP)
    import random; rng = random.Random(7)
    for _ in range(20):
        x, y = rng.randint(0, T-1), rng.randint(0, T-1)
        px(img, x, y, DD)
    return img

def tile_ground_rock_TC():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 7, T-1, T-1, RM)
    hline(img, 6, 0, T-1, RD)
    hline(img, 5, 0, T-1, RL)
    hline(img, 4, 0, T-1, RL)
    hline(img, 3, 0, T-1, RS)
    rock_texture(img, 8, T-1)
    # rock surface detail - pebbles
    for x in [5, 12, 20, 27]:
        px(img, x, 5, RL)
        px(img, x+1, 5, RL)
    return img

def tile_ground_rock_TL():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 6, 7, T-1, T-1, RM)
    rect(d, 6, 4, T-1, 6, RL)
    rect(d, 6, 3, T-1, 4, RS)
    for y in range(7, T):
        px(img, 5, y, RD)
        px(img, 4, y, RL)
    px(img, 5, 6, RD)
    rock_texture(img, 8, T-1, 7, T-1)
    return img

def tile_ground_rock_TR():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 7, T-7, T-1, RM)
    rect(d, 0, 4, T-7, 6, RL)
    rect(d, 0, 3, T-7, 4, RS)
    for y in range(7, T):
        px(img, T-6, y, RD)
        px(img, T-5, y, RL)
    px(img, T-6, 6, RD)
    rock_texture(img, 8, T-1, 0, T-8)
    return img

def tile_ground_sand_BC():
    """Bottom of a hanging sand ledge / overhang."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-7, SM)
    hline(img, T-6, 0, T-1, SE)
    hline(img, T-5, 0, T-1, SD)
    hline(img, T-4, 0, T-1, SD)
    # drip detail
    for x in [6, 14, 22]:
        px(img, x, T-3, SD)
        px(img, x, T-2, SD)
    sand_texture(img, 0, T-8)
    return img

def tile_ground_rock_BC():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-7, RM)
    hline(img, T-6, 0, T-1, RD)
    hline(img, T-5, 0, T-1, RS)
    hline(img, T-4, 0, T-1, RS)
    rock_texture(img, 0, T-8)
    return img

def tile_transition_sand_rock():
    """Sand on top blending into rock below."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, 15, SM)
    rect(d, 0, 16, T-1, T-1, RM)
    hline(img, 14, 0, T-1, SD)
    hline(img, 15, 0, T-1, RD)
    sand_texture(img, 0, 14)
    rock_texture(img, 17, T-1)
    return img

def tile_corner_out_TL():
    """Outer corner: sand, solid bottom-right, transparent top-left."""
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        for x in range(T):
            if x >= y + 4:
                col = SM if y > 4 else SL if y > 2 else None
                if col:
                    px(img, x, y, col)
    # edge highlight
    for y in range(T-5):
        px(img, y+4, y, SE)
        if y+3 < T:
            px(img, y+3, y, SL)
    sand_texture(img)
    return img

def tile_corner_out_TR():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        for x in range(T):
            if x <= T-5-y:
                col = SM if y > 4 else SL if y > 2 else None
                if col:
                    px(img, x, y, col)
    for y in range(T-5):
        px(img, T-5-y, y, SE)
        if T-4-y >= 0:
            px(img, T-4-y, y, SL)
    sand_texture(img)
    return img

def tile_corner_in_TL():
    """Inner concave corner."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 6, 0, T-1, T-1, SM)
    rect(d, 0, 6, 5, T-1, SM)
    sand_texture(img)
    hline(img, 5, 6, T-1, SE)
    hline(img, 4, 6, T-1, SL)
    vline(img, 5, 6, T-1, SE)
    vline(img, 4, 6, T-1, SL)
    px(img, 5, 5, SE)
    return img

def tile_corner_in_TR():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-7, T-1, SM)
    rect(d, T-6, 6, T-1, T-1, SM)
    sand_texture(img)
    hline(img, 5, 0, T-7, SE)
    hline(img, 4, 0, T-7, SL)
    vline(img, T-6, 6, T-1, SE)
    vline(img, T-5, 6, T-1, SL)
    px(img, T-6, 5, SE)
    return img

def tile_slope_sand_L():
    """Left-rising sand slope (ramps up left to right)."""
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        w = int((T-y) * T / T)
        if w > 0:
            rect(d, T-w, y, T-1, y, SM)
    # highlight
    for y in range(T):
        x = T - int((T-y))
        px(img, x, y, SL)
        px(img, x+1, y, SE)
    sand_texture(img)
    return img

def tile_slope_sand_R():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        w = int((T-y) * T / T)
        if w > 0:
            rect(d, 0, y, w-1, y, SM)
    for y in range(T):
        x = int((T-y))-1
        if 0 <= x < T:
            px(img, x, y, SL)
            if x-1 >= 0:
                px(img, x-1, y, SE)
    sand_texture(img)
    return img

def tile_slope_rock_L():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        w = int((T-y) * T / T)
        if w > 0:
            rect(d, T-w, y, T-1, y, RM)
    for y in range(T):
        x = T - int((T-y))
        px(img, x, y, RL)
        if x+1 < T:
            px(img, x+1, y, RD)
    rock_texture(img)
    return img

def tile_slope_rock_R():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        w = int((T-y) * T / T)
        if w > 0:
            rect(d, 0, y, w-1, y, RM)
    for y in range(T):
        x = int((T-y))-1
        if 0 <= x < T:
            px(img, x, y, RL)
            if x-1 >= 0:
                px(img, x-1, y, RD)
    rock_texture(img)
    return img

# =====================================================================
# PLATFORM TILES
# =====================================================================

def tile_plat_rock_L():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 4, 12, T-1, 20, RM)
    hline(img, 11, 4, T-1, RL)
    hline(img, 10, 4, T-1, RL)
    hline(img, 21, 4, T-1, RD)
    hline(img, 22, 4, T-1, RS)
    # left cap
    for y in range(10, 23):
        px(img, 3, y, RD)
        px(img, 2, y, RL)
    px(img, 3, 10, RL)
    rock_texture(img, 12, 20, 5, T-1)
    return img

def tile_plat_rock_C():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 12, T-1, 20, RM)
    hline(img, 11, 0, T-1, RL)
    hline(img, 10, 0, T-1, RL)
    hline(img, 21, 0, T-1, RD)
    hline(img, 22, 0, T-1, RS)
    rock_texture(img, 12, 20)
    return img

def tile_plat_rock_R():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 12, T-5, 20, RM)
    hline(img, 11, 0, T-5, RL)
    hline(img, 10, 0, T-5, RL)
    hline(img, 21, 0, T-5, RD)
    hline(img, 22, 0, T-5, RS)
    for y in range(10, 23):
        px(img, T-4, y, RD)
        px(img, T-3, y, RL)
    px(img, T-4, 10, RL)
    rock_texture(img, 12, 20, 0, T-6)
    return img

def tile_plat_single_rock():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 3, 12, T-4, 20, RM)
    hline(img, 11, 3, T-4, RL)
    hline(img, 10, 4, T-5, RL)
    hline(img, 21, 3, T-4, RD)
    hline(img, 22, 4, T-5, RS)
    vline(img, 2, 12, 20, RD)
    vline(img, T-3, 12, 20, RD)
    rock_texture(img, 12, 20, 4, T-5)
    return img

def tile_plat_ancient_L():
    """Ancient carved stone platform - left end."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 4, 11, T-1, 21, AM)
    hline(img, 10, 4, T-1, AL)
    hline(img, 9, 5, T-1, AL)
    hline(img, 22, 4, T-1, AD)
    hline(img, 23, 4, T-1, AD)
    # carved detail
    for x in range(8, T-2, 8):
        vline(img, x, 13, 20, AD)
    # left cap
    for y in range(9, 24):
        px(img, 3, y, AD)
        px(img, 2, y, AM)
    px(img, 3, 9, AL)
    # glyph hint
    px(img, 12, 15, AD)
    px(img, 13, 14, AD)
    px(img, 13, 16, AD)
    return img

def tile_plat_ancient_C():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 11, T-1, 21, AM)
    hline(img, 10, 0, T-1, AL)
    hline(img, 9, 0, T-1, AL)
    hline(img, 22, 0, T-1, AD)
    hline(img, 23, 0, T-1, AD)
    for x in range(8, T, 8):
        vline(img, x, 13, 20, AD)
    px(img, 16, 15, AD); px(img, 17, 14, AD); px(img, 17, 16, AD)
    return img

def tile_plat_ancient_R():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 11, T-5, 21, AM)
    hline(img, 10, 0, T-5, AL)
    hline(img, 9, 0, T-6, AL)
    hline(img, 22, 0, T-5, AD)
    hline(img, 23, 0, T-5, AD)
    for x in range(8, T-4, 8):
        vline(img, x, 13, 20, AD)
    for y in range(9, 24):
        px(img, T-4, y, AD)
        px(img, T-3, y, AM)
    px(img, T-4, 9, AL)
    return img

def tile_plat_single_ancient():
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 4, 11, T-5, 21, AM)
    hline(img, 10, 4, T-5, AL)
    hline(img, 9, 5, T-6, AL)
    hline(img, 22, 4, T-5, AD)
    hline(img, 23, 4, T-5, AD)
    vline(img, 3, 11, 21, AD)
    vline(img, T-4, 11, 21, AD)
    px(img, 16, 15, AD); px(img, 16, 16, AD)
    return img

# =====================================================================
# BACKGROUND TILES
# =====================================================================

def tile_bg_sky_top():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        t = y / (T-1)
        r = int(SKT[0] + (SKM[0]-SKT[0])*t)
        g = int(SKT[1] + (SKM[1]-SKT[1])*t)
        b = int(SKT[2] + (SKM[2]-SKT[2])*t)
        hline(img, y, 0, T-1, (r,g,b))
    return img

def tile_bg_sky_mid():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        t = y / (T-1)
        r = int(SKM[0] + (SKB[0]-SKM[0])*t*0.5)
        g = int(SKM[1] + (SKB[1]-SKM[1])*t*0.5)
        b = int(SKM[2] + (SKB[2]-SKM[2])*t*0.5)
        hline(img, y, 0, T-1, (r,g,b))
    # faint dust haze streaks
    import random; rng = random.Random(3)
    for _ in range(3):
        y = rng.randint(5, T-5)
        x1 = rng.randint(0, 10)
        hline(img, y, x1, x1+rng.randint(8,20), HZC)
    return img

def tile_bg_sky_bottom():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        t = y / (T-1)
        r = int(SKB[0] + (HZC[0]-SKB[0])*t)
        g = int(SKB[1] + (HZC[1]-SKB[1])*t)
        b = int(SKB[2] + (HZC[2]-SKB[2])*t)
        hline(img, y, 0, T-1, (r,g,b))
    return img

def tile_bg_dunes_far():
    """Repeatable dune silhouette strip."""
    img = mk(); d = ImageDraw.Draw(img)
    # sky above
    for y in range(16):
        t = y / 15
        r = int(SKB[0]*(1-t) + HZC[0]*t)
        g = int(SKB[1]*(1-t) + HZC[1]*t)
        b = int(SKB[2]*(1-t) + HZC[2]*t)
        hline(img, y, 0, T-1, (r,g,b))
    # dune silhouette - simple sinusoidal shape
    import math
    for x in range(T):
        h = int(12 + 4 * math.sin(x * math.pi * 2 / T) + 2 * math.sin(x * math.pi * 5 / T))
        for y in range(h, T):
            if y == h:
                px(img, x, y, DF)
            elif y < h+3:
                px(img, x, y, DN)
            else:
                px(img, x, y, DK)
    return img

def tile_bg_cliff_face():
    """Flat cliff wall background texture."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-1, RM)
    rock_texture(img)
    # horizontal strata lines
    for y in [8, 16, 24]:
        hline(img, y, 0, T-1, RD)
        hline(img, y+1, 0, T-1, RL)
    return img

def tile_bg_pillar_L():
    img = mk(); d = ImageDraw.Draw(img)
    # sky background
    for y in range(T):
        t = y / (T-1)
        r = int(SKM[0] + (SKB[0]-SKM[0])*t*0.3)
        g = int(SKM[1] + (SKB[1]-SKM[1])*t*0.3)
        b = int(SKM[2] + (SKB[2]-SKM[2])*t*0.3)
        hline(img, y, 0, T-1, (r,g,b))
    # left side of distant pillar (dark silhouette)
    for y in range(T):
        rect(d := ImageDraw.Draw(img), 16, 0, T-1, T-1, (120,85,40))
    vline(img, 16, 0, T-1, (100,70,32))
    return img

def tile_bg_pillar_C():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        t = y / (T-1)
        r = int(SKM[0] + (SKB[0]-SKM[0])*t*0.3)
        g = int(SKM[1] + (SKB[1]-SKM[1])*t*0.3)
        b = int(SKM[2] + (SKB[2]-SKM[2])*t*0.3)
        hline(img, y, 0, T-1, (r,g,b))
    rect(ImageDraw.Draw(img), 0, 0, T-1, T-1, (120,85,40))
    vline(img, 0, 0, T-1, (100,70,32))
    vline(img, T-1, 0, T-1, (100,70,32))
    vline(img, 1, 0, T-1, (145,105,55))
    return img

def tile_bg_pillar_R():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(T):
        t = y / (T-1)
        r = int(SKM[0] + (SKB[0]-SKM[0])*t*0.3)
        g = int(SKM[1] + (SKB[1]-SKM[1])*t*0.3)
        b = int(SKM[2] + (SKB[2]-SKM[2])*t*0.3)
        hline(img, y, 0, T-1, (r,g,b))
    rect(ImageDraw.Draw(img), 0, 0, 15, T-1, (120,85,40))
    vline(img, 15, 0, T-1, (100,70,32))
    vline(img, 1, 0, T-1, (145,105,55))
    return img

# =====================================================================
# DECORATION TILES
# =====================================================================

def tile_deco_cactus_S():
    """Small 1-tile cactus."""
    img = mk(); d = ImageDraw.Draw(img)
    # trunk
    vline(img, 16, 10, T-1, CM)
    vline(img, 15, 10, T-1, CL)
    vline(img, 17, 10, T-1, CD)
    # arms
    hline(img, 18, 8, 13, CM); vline(img, 8, 14, 18, CM); vline(img, 13, 14, 18, CM)
    hline(img, 20, 18, 23, CM); vline(img, 23, 16, 20, CM)
    # tips
    px(img, 8, 13, CL); px(img, 13, 13, CL); px(img, 23, 15, CL)
    return img

def tile_deco_cactus_T_top():
    """Tall cactus - top half."""
    img = mk(); d = ImageDraw.Draw(img)
    vline(img, 16, 0, T-1, CM); vline(img, 15, 0, T-1, CL); vline(img, 17, 0, T-1, CD)
    # arms mid
    hline(img, 12, 8, 14, CM); vline(img, 8, 8, 14, CM); px(img, 8, 7, CL)
    hline(img, 16, 18, 24, CM); vline(img, 24, 12, 16, CM); px(img, 24, 11, CL)
    return img

def tile_deco_cactus_T_bot():
    """Tall cactus - bottom half (connects to ground)."""
    img = mk(); d = ImageDraw.Draw(img)
    vline(img, 16, 0, T-1, CM); vline(img, 15, 0, T-1, CL); vline(img, 17, 0, T-1, CD)
    return img

def tile_deco_drybush():
    img = mk(); d = ImageDraw.Draw(img)
    # branches spread from center-bottom
    base_y = T-5
    for x, y in [(16,base_y),(12,base_y-4),(20,base_y-4),(9,base_y-8),(24,base_y-8),(14,base_y-8),(19,base_y-7)]:
        px(img, x, y, WD); px(img, x-1, y, WDK)
    for x in range(10, 22):
        if (x+base_y) % 3 == 0:
            px(img, x, base_y-2, (80,110,40)); px(img, x, base_y-3, (70,100,35))
    vline(img, 16, base_y-6, base_y, WDK)
    return img

def tile_deco_bones():
    img = mk()
    bone = (220, 210, 185)
    bone_d = (180, 165, 140)
    # skull
    for x in range(12,20):
        for y in range(14,21):
            px(img, x, y, bone)
    for p in [(13,15),(14,15),(15,15),(16,15),(17,15),(18,15),(12,16),(19,16),(12,17),(19,17),(14,18),(17,18)]:
        px(img, *p, bone_d)
    # jaw opening
    rect(ImageDraw.Draw(img), 14, 19, 17, 20, (0,0,0,200))
    # scattered bones
    hline(img, T-6, 4, 12, bone); hline(img, T-7, 5, 11, bone_d)
    hline(img, T-5, 18, 26, bone); hline(img, T-6, 19, 25, bone_d)
    return img

def tile_deco_urn_S():
    """Small cracked clay urn."""
    img = mk(); d = ImageDraw.Draw(img)
    urn_l = (195, 118, 65); urn_m = (165, 88, 45); urn_d = (120, 62, 30)
    # body
    for y in range(14, T-4):
        w = max(1, int(7 * abs(1 - (y-14)/10.0) + 3))
        if y > 22: w = max(2, int(5 - (y-22)))
        cx = 16
        rect(d, cx-w, y, cx+w, y, urn_m)
        px(img, cx-w, y, urn_l); px(img, cx+w, y, urn_d)
    # rim
    hline(img, 13, 11, 20, urn_l); hline(img, 12, 12, 19, urn_d)
    # crack
    for y in range(16, 24):
        if y % 2 == 0: px(img, 18, y, urn_d)
    return img

def tile_deco_rock_S():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(18, T-3):
        w = max(1, int(6*(1-abs((y-22)/6.0))))
        rect(d, 14-w, y, 14+w+1, y, RM)
        px(img, 14-w, y, RL); px(img, 14+w+1, y, RD)
    return img

def tile_deco_rock_M():
    img = mk(); d = ImageDraw.Draw(img)
    for y in range(12, T-2):
        w = max(1, int(9*(1-abs((y-20)/10.0))))
        rect(d, 16-w, y, 16+w, y, RM)
        px(img, 16-w, y, RL); px(img, 16+w, y, RD)
    # shadow
    hline(img, T-2, 10, 22, RD)
    return img

def tile_deco_sandpile():
    img = mk(); d = ImageDraw.Draw(img)
    import math
    for x in range(T):
        h = int(8 * (1 - ((x-16)/14)**2)) if abs(x-16) < 14 else 0
        for y in range(T-h, T):
            if y == T-h:
                px(img, x, y, SL)
            elif y < T-h+2:
                px(img, x, y, SM)
            else:
                px(img, x, y, SD)
    return img

def tile_deco_carving():
    """Ancient wall glyph/carving."""
    img = mk(); d = ImageDraw.Draw(img)
    rect(d, 0, 0, T-1, T-1, RM)
    rock_texture(img)
    # carved symbol (sunburst-ish glyph)
    cx, cy = 16, 16
    for r in range(3):
        px(img, cx+r, cy, AD); px(img, cx-r, cy, AD)
        px(img, cx, cy+r, AD); px(img, cx, cy-r, AD)
    for dx,dy in [(-4,-4),(4,-4),(-4,4),(4,4)]:
        vline(img, cx+dx, cy+dy-2, cy+dy+2, AD)
        hline(img, cy+dy, cx+dx-2, cx+dx+2, AD)
    hline(img, cy, cx-8, cx+8, AD)
    vline(img, cx, cy-8, cy+8, AD)
    return img

def tile_deco_torch():
    img = mk(); d = ImageDraw.Draw(img)
    # wall mount
    rect(d, 13, 16, 19, T-2, WDK)
    vline(img, 12, 16, T-2, WD)
    # flame
    for x,y,c in [(15,8,(255,200,50)),(16,6,(255,160,30)),(17,8,(255,220,80)),
                  (15,10,(255,140,20)),(16,9,(255,180,40)),(17,10,(255,120,10)),
                  (16,13,(200,100,20)),(15,12,(220,120,30)),(17,12,(180,80,15))]:
        px(img, x, y, c)
    # handle wrap
    hline(img, 16, 13, 18, (80,60,30))
    return img

def tile_deco_deadtree_top():
    img = mk(); d = ImageDraw.Draw(img)
    vline(img, 16, 0, T-1, WDK); vline(img, 15, 0, T-1, WD); vline(img, 17, 0, T-1, (70,50,30))
    # branches
    for y,x1,x2 in [(6,4,14),(10,18,26),(4,14,20)]:
        hline(img, y, x1, x2, WDK)
        hline(img, y-1, x1+2, x2-2, WD)
    for x in [4,5,18,19,25,26]:
        vline(img, x, 4, 8, WDK)
    return img

def tile_deco_deadtree_bot():
    img = mk(); d = ImageDraw.Draw(img)
    vline(img, 16, 0, T-1, WDK); vline(img, 15, 0, T-1, WD); vline(img, 17, 0, T-1, (70,50,30))
    # roots at bottom
    for x,y in [(12,T-3),(10,T-2),(20,T-3),(22,T-2)]:
        px(img, x, y, WDK); px(img, x-1, y, WD)
    return img

# =====================================================================
# TILESET ASSEMBLY
# =====================================================================

TILES = [
    # Row 0: ground sand
    tile_ground_sand_TL, tile_ground_sand_TC, tile_ground_sand_TR,
    tile_fill_sand, tile_corner_out_TL, tile_corner_out_TR,
    tile_corner_in_TL, tile_corner_in_TR,
    # Row 1: ground rock + slopes
    tile_ground_rock_TL, tile_ground_rock_TC, tile_ground_rock_TR,
    tile_fill_rock, tile_slope_sand_L, tile_slope_sand_R,
    tile_slope_rock_L, tile_slope_rock_R,
    # Row 2: undersides + dark fill + transition
    tile_ground_sand_BC, tile_ground_sand_BC, tile_ground_sand_BC,  # BL/BC/BR simplified
    tile_fill_dark, tile_ground_rock_BC, tile_ground_rock_BC, tile_ground_rock_BC,
    tile_transition_sand_rock,
    # Row 3: platforms
    tile_plat_rock_L, tile_plat_rock_C, tile_plat_rock_R, tile_plat_single_rock,
    tile_plat_ancient_L, tile_plat_ancient_C, tile_plat_ancient_R, tile_plat_single_ancient,
    # Row 4: background tiles
    tile_bg_sky_top, tile_bg_sky_mid, tile_bg_sky_bottom, tile_bg_dunes_far,
    tile_bg_cliff_face, tile_bg_pillar_L, tile_bg_pillar_C, tile_bg_pillar_R,
    # Row 5: decorations part 1
    tile_deco_cactus_S, tile_deco_cactus_T_top, tile_deco_cactus_T_bot, tile_deco_drybush,
    tile_deco_bones, tile_deco_urn_S, tile_deco_rock_S, tile_deco_rock_M,
    # Row 6: decorations part 2
    tile_deco_sandpile, tile_deco_carving, tile_deco_torch,
    tile_deco_deadtree_top, tile_deco_deadtree_bot,
    tile_bg_pillar_L, tile_bg_pillar_C, tile_bg_pillar_R,  # extra bg tiles
]

ROWS = (len(TILES) + COLS - 1) // COLS
tileset = Image.new('RGBA', (COLS * T, ROWS * T), (0, 0, 0, 0))

for i, fn in enumerate(TILES):
    col = i % COLS
    row = i // COLS
    tile_img = fn()
    tileset.paste(tile_img, (col * T, row * T))

tileset.save(os.path.join(OUT, 'tileset.png'))
print(f"Saved tileset.png ({COLS*T}x{ROWS*T})")

# =====================================================================
# BACKGROUND.PNG  —  repeatable mid-section (256 wide x 256 tall)
# =====================================================================

BW, BH = 256, 256
bg = Image.new('RGBA', (BW, BH), (0,0,0,255))
bd = ImageDraw.Draw(bg)

import math

# Sky gradient
for y in range(BH):
    t = y / (BH-1)
    if t < 0.6:
        tt = t / 0.6
        r = int(SKT[0] + (SKM[0]-SKT[0])*tt)
        g = int(SKT[1] + (SKM[1]-SKT[1])*tt)
        b = int(SKT[2] + (SKM[2]-SKT[2])*tt)
    else:
        tt = (t-0.6)/0.4
        r = int(SKM[0] + (SKB[0]-SKM[0])*tt)
        g = int(SKM[1] + (SKB[1]-SKM[1])*tt)
        b = int(SKM[2] + (SKB[2]-SKM[2])*tt)
    bd.line([(0,y),(BW-1,y)], fill=(r,g,b,255))

# Dust haze streaks
import random; rng = random.Random(99)
for _ in range(18):
    y = rng.randint(20, BH-40)
    x1 = rng.randint(0, 100)
    x2 = rng.randint(x1+20, min(x1+120, BW-1))
    alpha = rng.randint(15, 35)
    overlay = Image.new('RGBA', (BW, BH), (0,0,0,0))
    ImageDraw.Draw(overlay).line([(x1,y),(x2,y)], fill=HZC+(alpha,), width=1)
    bg = Image.alpha_composite(bg, overlay)

# Distant rock pillars - three sets across width
pillar_positions = [30, 110, 190]
for px_off in pillar_positions:
    pw = rng.randint(16, 28)
    ph = rng.randint(80, 140)
    py_start = rng.randint(30, 80)
    pillar_col = (int(DK[0]*0.7), int(DK[1]*0.7), int(DK[2]*0.7), 200)
    pillar_hi  = (int(DF[0]*0.8), int(DF[1]*0.8), int(DF[2]*0.8), 200)
    overlay = Image.new('RGBA', (BW, BH), (0,0,0,0))
    od = ImageDraw.Draw(overlay)
    od.rectangle([px_off, py_start, px_off+pw, py_start+ph], fill=pillar_col)
    # highlight left edge
    od.line([(px_off+1, py_start), (px_off+1, py_start+ph)], fill=pillar_hi)
    # top cap widening
    od.rectangle([px_off-4, py_start, px_off+pw+4, py_start+8], fill=pillar_col)
    bg = Image.alpha_composite(bg, overlay)

# Distant dune line at bottom
dune_ov = Image.new('RGBA', (BW, BH), (0,0,0,0))
dd2 = ImageDraw.Draw(dune_ov)
for x in range(BW):
    h = int(15 + 8*math.sin(x*math.pi*2/BW*1.5) + 5*math.sin(x*math.pi*2/BW*3.7))
    y0 = BH - 30 - h
    for y in range(y0, BH):
        alpha = 200 if y == y0 else 230
        col = DN if y < y0+5 else DK
        dune_ov.putpixel((x,y), col+(alpha,))
bg = Image.alpha_composite(bg, dune_ov)

bg = bg.convert('RGB')
bg.save(os.path.join(OUT, 'background.png'))
print(f"Saved background.png ({BW}x{BH})")

# =====================================================================
# FINALBACKGROUND.PNG  —  top-cap (256 wide x 256 tall)
# The "apex" of the vertical platformer — bright sun, dramatic sky
# =====================================================================

FW, FH = 256, 256
fb = Image.new('RGBA', (FW, FH), (0,0,0,255))
fd = ImageDraw.Draw(fb)

# Bright apex sky — bleached yellow-white at top fading to warm orange
for y in range(FH):
    t = y / (FH-1)
    top_col = (248, 230, 130)
    bot_col  = SKT
    r = int(top_col[0] + (bot_col[0]-top_col[0])*t)
    g = int(top_col[1] + (bot_col[1]-top_col[1])*t)
    b = int(top_col[2] + (bot_col[2]-top_col[2])*t)
    fd.line([(0,y),(FW-1,y)], fill=(r,g,b,255))

# Sun — large, bleached, centered near top
sun_cx, sun_cy = FW//2, 55
sun_r = 28
sun_ov = Image.new('RGBA', (FW, FH), (0,0,0,0))
sd = ImageDraw.Draw(sun_ov)

# Outer glow rings
for radius, alpha in [(sun_r+20,30),(sun_r+13,50),(sun_r+7,80),(sun_r+3,120)]:
    sd.ellipse([sun_cx-radius, sun_cy-radius, sun_cx+radius, sun_cy+radius],
               fill=SUNR+(alpha,))
# Main sun body
sd.ellipse([sun_cx-sun_r, sun_cy-sun_r, sun_cx+sun_r, sun_cy+sun_r], fill=SUN+(255,))
# Highlight
sd.ellipse([sun_cx-sun_r+5, sun_cy-sun_r+5, sun_cx+4, sun_cy+4], fill=SUH+(200,))

# Sun rays
for angle in range(0, 360, 22):
    rad = math.radians(angle)
    x1 = int(sun_cx + (sun_r+5)*math.cos(rad))
    y1 = int(sun_cy + (sun_r+5)*math.sin(rad))
    x2 = int(sun_cx + (sun_r+18)*math.cos(rad))
    y2 = int(sun_cy + (sun_r+18)*math.sin(rad))
    sd.line([(x1,y1),(x2,y2)], fill=SUN+(160,), width=2)

fb = Image.alpha_composite(fb, sun_ov)

# High-altitude thin haze bands
rng2 = random.Random(55)
for _ in range(12):
    y = rng2.randint(10, FH//2)
    x1 = rng2.randint(0, 60)
    x2 = rng2.randint(x1+30, min(x1+160, FW-1))
    alpha = rng2.randint(20, 50)
    hz_ov = Image.new('RGBA', (FW, FH), (0,0,0,0))
    ImageDraw.Draw(hz_ov).line([(x1,y),(x2,y)], fill=(255,240,180,alpha), width=1)
    fb = Image.alpha_composite(fb, hz_ov)

# Distant jagged mountain ridge silhouette across lower half
ridge_ov = Image.new('RGBA', (FW, FH), (0,0,0,0))
rd2 = ImageDraw.Draw(ridge_ov)
rng3 = random.Random(77)
ridge_y = [0]*FW

# Build ridge profile
base_h = FH - 80
for i in range(FW):
    ridge_y[i] = base_h + int(20*math.sin(i*0.06) + 15*math.sin(i*0.11+1) + 10*math.sin(i*0.19+2))

# Draw ridge
ridge_col_L = (int(DK[0]*0.6), int(DK[1]*0.55), int(DK[2]*0.5), 220)
ridge_col_R = (int(DK[0]*0.4), int(DK[1]*0.38), int(DK[2]*0.35), 240)
for x in range(FW):
    ry = ridge_y[x]
    # left-lit face
    is_rising = (x > 0 and ridge_y[x] < ridge_y[x-1])
    col = ridge_col_L if is_rising else ridge_col_R
    for y in range(ry, FH):
        ridge_ov.putpixel((x,y), col)

fb = Image.alpha_composite(fb, ridge_ov)

# Sand/desert floor band at very bottom
floor_ov = Image.new('RGBA', (FW, FH), (0,0,0,0))
fd2 = ImageDraw.Draw(floor_ov)
fd2.rectangle([0, FH-22, FW-1, FH-1], fill=SM+(255,))
fd2.line([(0,FH-23),(FW-1,FH-23)], fill=SL+(255,), width=2)
fd2.line([(0,FH-21),(FW-1,FH-21)], fill=SD+(180,), width=1)
# sand texture
for x in range(0, FW, 3):
    y = FH - 22 + rng3.randint(0,5)
    if y < FH:
        floor_ov.putpixel((x,y), SL+(200,))
fb = Image.alpha_composite(fb, floor_ov)

fb = fb.convert('RGB')
fb.save(os.path.join(OUT, 'finalbackground.png'))
print(f"Saved finalbackground.png ({FW}x{FH})")

print("Done.")
