# robotic-arm-app
🤖


| |         Name        |ID|        Params            | Description                                                     |   
|-|---------------------|--|--------------------------|-----------------------------------------------------------------|
|x|**RotateRelative**     || (angle, shoulder, time)  | Поворот указанного плеча shoulder, на угол angle за время time  |
|x|**RotateAbsolute**     || (angle, shoulder, time)  | Установка указанного плеча shoulder, в угол angle за время time |
|x|**RotateAllRelative**  || (angles[], time)         | Поворот всех плеч на углы angles за время time                  |
|x|**RotateAllAbsolute**  || (angles[], time)         | Установка всех плеч в углы angles за время time                 |
|x|**MoveToPointRelative**|| (Δx, Δy, Δz, time        | Перемещение манипулятора в координаты x, y, z за время time     |
|x|**MoveToPointAbsolute**|| (x, y, z, time)          | Перемещение манипулятора на Δx, Δy, Δz за время time            |
|x|**GripperState**       || Open \| Closed           | Установка клешни в состояние открыто или закрыто                |
|x|**OpenGripper**        || Percentage               | Открытие клешни на percentage процентов                         |
