import json
import pandas as pd
from tqdm import tqdm
def repeat_anomaly(ext2, json_file='delay_anomaly_report.json'):
    with open(json_file, 'w', encoding='utf8') as f:
        anomaly_dict = {'violation':[], 'mistake':[]}
        tmp = ext2[ext2.volume > 0].sort_values(by='volume')
        for i in tqdm(range(tmp.shape[0] - 1)):
            lines = tmp.iloc[i:i+2]
            c1 = lines.volume.iloc[0] == lines.volume.iloc[1]
            c2 = lines.unit.iloc[0] != lines.unit.iloc[1]
            c3 = pd.Timedelta(lines.date_vsd.iloc[0] - lines.date_vsd.iloc[1]).seconds / 3600.0 < 1
            c4 = lines.fish.iloc[0] == lines.fish.iloc[1]
            if c1 & c2 & c3 & c4:
                anomaly_dict['mistake'].append(lines[['id_vsd', 'date_vsd']].to_json())          
            if (c1 & c2 & c4) and not c3:
                anomaly_dict['violation'].append(lines[['id_vsd', 'date_vsd']].to_json())          
        json.dump(anomaly_dict, f)