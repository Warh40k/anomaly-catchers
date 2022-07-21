# 7hours

# Anomaly catchers

Запуск файла main (для реализации в консоле main.py):
 
Выбрать date_from, date_to – период выборки; path_to_db – путь до данных; json_file – путь для сохранения json файлов "машиночитаемых" отчетов, 
anomaly_type – тип аномалии, которую хотим найти ('1e' - дубликаты по ключу, '2e' - повторный отчет, '3e' - отсутствие ВСД)

![image](https://user-images.githubusercontent.com/69635204/170849703-1334c992-268d-43c2-994d-631949dd9caf.png)

В результате появятся файлы anomaly.txt (отчет для оператора) и anomaly.json (для дальнейшей работы в программе)

GUI-приложение расположено в gui/test-MVVM.sln
