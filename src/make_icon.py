# иконка установщика dmxON Setup: значок надстройки (маска + медное кольцо) со стопкой слоёв и стрелкой установки
import os
from PIL import Image, ImageDraw

HERE = os.path.dirname(os.path.abspath(__file__))
CU0, CU1 = (180, 116, 74), (226, 171, 127)
MASK0, MASK1 = (23, 58, 50), (36, 87, 74)
WHITE = (243, 238, 231)
S = 512


def draw():
    im = Image.new('RGBA', (S, S), (0, 0, 0, 0))
    d = ImageDraw.Draw(im)
    d.rounded_rectangle((16, 16, S - 16, S - 16), radius=104, fill=MASK0, outline=CU0, width=22)
    # стопка слоёв платы: два ромба
    cx = S // 2
    for k, (col, dy) in enumerate(((MASK1, 92), (CU0, 40))):
        y = 214 + dy
        d.polygon([(cx, y - 70), (cx + 150, y), (cx, y + 70), (cx - 150, y)], fill=col, outline=CU1 if k else None)
    # стрелка установки
    d.rectangle((cx - 26, 70, cx + 26, 200), fill=WHITE)
    d.polygon([(cx - 78, 186), (cx + 78, 186), (cx, 270)], fill=WHITE)
    return im


big = draw()
sizes = [(256, 256), (128, 128), (64, 64), (48, 48), (32, 32), (24, 24), (16, 16)]
big.resize((256, 256), Image.LANCZOS).save(os.path.join(HERE, 'setup.ico'), sizes=sizes)
big.resize((128, 128), Image.LANCZOS).save(os.path.join(HERE, 'setup_logo.png'))
print('ok')
