# Data-Visualizer

Data Visualizer using WPF (Windows Presentation Foundation). The program is capable to visualize an object rotation in real time
and the object rotation, which occured in the past.

The operation of the program consists of:
1. Receiving data. Data is obtained from sensors (gyroscope and accelerometer) or from file.
2. Data processing. Once data is received, they are processed by Butterworth and Madgwick algorithms.
3. Data visualization. When data is processed, it is used to rotate the object.

A short video that shows the real-time change of the object:

[![Watch the video](http://www.mediafire.com/convkey/c537/46977a2zdzb9djqzg.jpg)](https://youtu.be/0KdEw85vVMw)
