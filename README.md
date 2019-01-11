# Data-Visualizer

Data Visualizer using Windows WPF. The program is capable to visualize an object rotation in real time
and the object rotation, which occured in the past.

The operation of the program consists of:
1. Receiving data. Data is obtained from sensors (gyroscope and accelerometer) or from file.
2. Data processing. Once data is received, they are processed by Butterworth and Madgwick algorithms.
3. Data visualization. When data is processed, it is used to rotate the object.

A short video that shows the real-time change of the object:

[![Watch the video](https://i.imgur.com/vKb2F1B.png)](https://www.youtube.com/watch?v=5YRYtuvI1U0)
