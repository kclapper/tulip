# Generating a Model for Tulip Bot

There are two options to enable the AI chat system, Tulip Bot.
One of them uses the ChatGPT API and requires a ChatGPT API key. 
This method is simple to use and requires no disk space on the
server.

The second method is to use a [LLaMa 2 model from Meta](https://llama.meta.com/).
This project uses a libary called [LLaMaSharp](https://github.com/SciSharp/LLamaSharp) 
in order to actually run these models. This library requires the models to be converted 
to the `gguf` format before it can run them. This document gives some instruction on 
how to accomplish this.

# Obtaining the LLaMa 2 Model

The LLaMa 2 model can be obtained by agreeing to the usage license and 
downloading it from [Meta's LLaMa website](https://llama.meta.com/llama-downloads).

# Process for Preparing the LLaMa 2 Model

This process is explained in the [llama.cpp documentation](https://github.com/ggerganov/llama.cpp?tab=readme-ov-file#prepare-and-quantize)
and is reproduced below. The license for llama.cpp and it's README documentation is included in this documents markdown source as a comment.

<!-- llama.cpp license
MIT License

Copyright (c) 2023-2024 The ggml authors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
-->

    # obtain the official LLaMA model weights and place them in ./models
    ls ./models
    llama-2-7b tokenizer_checklist.chk tokenizer.model
    # [Optional] for models using BPE tokenizers
    ls ./models
    <folder containing weights and tokenizer json> vocab.json
    # [Optional] for PyTorch .bin models like Mistral-7B
    ls ./models
    <folder containing weights and tokenizer json>

    # install Python dependencies
    python3 -m pip install -r requirements.txt

    # convert the model to ggml FP16 format
    python3 convert.py models/mymodel/

    # [Optional] for models using BPE tokenizers
    python convert.py models/mymodel/ --vocab-type bpe

    # quantize the model to 4-bits (using Q4_K_M method)
    ./quantize ./models/mymodel/ggml-model-f16.gguf ./models/mymodel/ggml-model-Q4_K_M.gguf Q4_K_M

    # update the gguf filetype to current version if older version is now unsupported
    ./quantize ./models/mymodel/ggml-model-Q4_K_M.gguf ./models/mymodel/ggml-model-Q4_K_M-v2.gguf COPY

# Using the Model

Once the model has been prepared by converting it to the `gguf`
format, it can be uploaded and enabled in the "Chat" section of
the admin panel.