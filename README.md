HandPoseBarracuda
=================

![gif](https://i.imgur.com/jvHmCMc.gif)
![gif](https://i.imgur.com/KZmAcPa.gif)

**HandPoseBarracuda** is a proof-of-concept implementation of a neural network
hand/finger tracker that works with a monocular color camera.

Basically, HandPoseBarracuda is a partial port of the [MediaPipe Hands]
pipeline. Although it is not a straight port of the original package, it uses
the same basic design and the same pre-trained models.

[MediaPipe Hands]: https://google.github.io/mediapipe/solutions/hands.html

Note that this is just a proof-of-concept implementation. It lacks some
essential features needed for practical applications:

- **It only accepts a single hand.** Although you can reuse the most part of
  the implementation, you will need to redesign the system to support multiple
  hands.
- **It only supports screen-space (2D) positions and relative depths from
  a palm.** You will need to implement a screen-to-world-space projector for 3D
  applications.

Related projects
----------------

HandPoseBarracuda uses the following sub-packages:

- [BlazePalmBarracuda (lightweight hand detector)](https://github.com/keijiro/BlazePalmBarracuda)
- [HandLandmarkBarracuda (hand/finger landmark detector)](https://github.com/keijiro/HandLandmarkBarracuda)

