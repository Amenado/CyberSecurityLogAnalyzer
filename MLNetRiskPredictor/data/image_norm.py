import pandas as pd

df = pd.read_csv("C:/Users/BoraErki/Desktop/CyberSecurityLogAnalyzer/MLNetRiskPredictor/data/Friday-WorkingHours-Afternoon-DDos.pcap_ISCX.csv")

df[" Label"] = df[" Label"].map({"BENIGN": 0, "DDoS": 1})

print(df[" Label"].value_counts())

df.to_csv("benign_ddos.csv", index=False)
