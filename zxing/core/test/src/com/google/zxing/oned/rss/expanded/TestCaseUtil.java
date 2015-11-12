/*
 * Copyright (C) 2012 ZXing authors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*
 * These authors would like to acknowledge the Spanish Ministry of Industry,
 * Tourism and Trade, for the support in the project TSI020301-2008-2
 * "PIRAmIDE: Personalizable Interactions with Resources on AmI-enabled
 * Mobile Dynamic Environments", led by Treelogic
 * ( http://www.treelogic.com/ ):
 *
 *   http://www.piramidepse.com/
 */

package com.google.zxing.oned.rss.expanded;

import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;

import com.google.zxing.BinaryBitmap;
import com.google.zxing.BufferedImageLuminanceSource;
import com.google.zxing.common.GlobalHistogramBinarizer;

final class TestCaseUtil {

  private TestCaseUtil() {
  }

  static BufferedImage getBufferedImage(String path) throws IOException {
    File file = new File(path);
    if (!file.exists()) {
      // Support running from project root too
      file = new File("core", path);
    }
    return ImageIO.read(file);
	}
	
  static BinaryBitmap getBinaryBitmap(String path) throws IOException {
		BufferedImage bufferedImage = getBufferedImage(path);
		BufferedImageLuminanceSource luminanceSource = new BufferedImageLuminanceSource(bufferedImage);
		return new BinaryBitmap(new GlobalHistogramBinarizer(luminanceSource));
	}

}
