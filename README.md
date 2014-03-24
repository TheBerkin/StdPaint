StdPaint
========


StdPaint provides a fast, flexible way to directly access and manipulate the console buffer. It uses a double-buffered, multithreaded system to make sure that rendering is as fast as possible.

Features
-----
* Several built-in drawing methods, such as circle, box, line, and flood fill
* Create additional buffers for drawing graphics for later use
* Mouse event handling with buffer coordinates
* Controllable refresh rate
* Text drawing with alignment options
* Buffers can be saved to/loaded from files and used like texture assets
