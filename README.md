# Portrait-mode
High quality implementation of the "portrait mode" effect using Neural Networks.

## How it works?
Traditionally, the portrait mode effect has been achieved using 2 lenses which detect objects present in the foreground and in the background. With advances in the field of ML, this effect can also be implemented using only image segmentation. Using the pretrained [**DeepLab-v3+**](https://github.com/tensorflow/models/tree/master/research/deeplab) open source TensorFlow model, we can find the objects in the foreground of the image and blur the background to replicate this effect.

## DeepLab to ONNX
Download DeepLab model trained on [**COCO dataset**](https://cocodataset.org/#home) from the [**Model Zoo**](https://github.com/tensorflow/models/blob/master/research/deeplab/g3doc/model_zoo.md) and convert it to onnx model.  
For example download [**deeplabv3_mnv2_pascal_train_aug_2018_01_29**](http://download.tensorflow.org/models/deeplabv3_mnv2_pascal_train_aug_2018_01_29.tar.gz) and convert it to onnx model from **frozen_inference_graph.pb**
```
python -m tf2onnx.convert 
  --opset 11
  --fold_const
  --graphdef frozen_inference_graph.pb
  --output deeplabv3_mnv2_pascal_train_aug.onnx
  --inputs ImageTensor:0
  --outputs SemanticPredictions:0
```
or use already-made [**onnx**](https://yadi.sk/d/SieS9IWAzYhdZg?w=1) model.

## C# application
fsfs
<p align="center"><img width="50%" src="docs/girl.jpg"/><img width="50%" src="docs/girl_effect.jpg"/></p>  
<p align="center"><b>Figure 1.</b> Example of "portrait mode" effect</p>  

<p align="center"><img width="50%" src="docs/girl2.jpg"/><img width="50%" src="docs/girl2_effect.jpg"/></p>  
<p align="center"><b>Figure 1.</b> Example of "portrait mode" effect</p>  
