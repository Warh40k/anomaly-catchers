import json
import pandas as pd
from tqdm import tqdm
def repeat_anomaly(ext2, txt_file='repeat_report_anomaly.txt', hours_delay=1):
    anomaly_count = 0
    anomaly_dict = {'violation':[], 'mistake':[]}
    tmp = ext2[ext2.volume > 0].sort_values(by='volume')
    for i in tqdm(range(tmp.shape[0] - 1)):
        lines = tmp.iloc[i:i+2]
        c1 = lines.volume.iloc[0] == lines.volume.iloc[1]
        c2 = lines.unit.iloc[0] != lines.unit.iloc[1]
        c3 = pd.Timedelta(lines.date_vsd.iloc[0] - lines.date_vsd.iloc[1]).seconds / 3600.0 < hours_delay
        c4 = lines.fish.iloc[0] == lines.fish.iloc[1]
        lines.date_vsd = lines.date_vsd.astype(str)
        if c1 & c2 & c3 & c4:
            anomaly_dict['mistake'].append(lines[['id_vsd', 'date_vsd', 'volume', 'unit', 'fish']].to_json()) 
            anomaly_count += 1         
        if (c1 & c2 & c4) and not c3:
            anomaly_dict['violation'].append(lines[['id_vsd', 'date_vsd', 'volume', 'unit', 'fish']].to_json())          
            anomaly_count += 1         
            

    # Человекочитаемый вид
    with open(txt_file, 'w', encoding='utf8') as f:
        print('Отчёт об аномалиях repeat_report\n')
        print(f'Выявлено {anomaly_count} аномалий\n', file=f)
        print(f'Задержки в пределах допустимого:\n', file=f)
        for line in anomaly_dict['mistake']:
            tmp = json.loads(line)
            print(pd.DataFrame(tmp).reset_index(drop=True), file=f) 
        print(file=f)
        print('-' * 40, file=f)
        print(file=f)
        print(f'Задержки больше {hours_delay} часа(ов):\n', file=f)
        for line in anomaly_dict['violation']:
            tmp = json.loads(line)
            print(pd.DataFrame(tmp).reset_index(drop=True), file=f)

    return anomaly_dict, anomaly_count
    