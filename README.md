### ITA - Descrizione dello script

## Aggiungi gli orari delle lezioni al calendario del telefono 📅

Lo script esegue le seguenti operazioni:

1. **Generazione URL**: Crea la URL per scaricare il file `.pdf` con gli orari della settimana corrente. Se è sabato o domenica, scarica quelli della settimana successiva.
   
2. **Estrazione immagine dal PDF**: Non essendo un file con testo, lo script estrae l'immagine degli orari dal PDF.

3. **Ritaglio e analisi immagine**: L'immagine viene ritagliata per prendere solo gli orari, estratti in base allo schema a colori usato dalla scuola.

4. **Creazione calendario**: Lo script genera un file `.ics` con gli orari delle lezioni.

5. **Invio del calendario**: Il file viene salvato localmente e inviato via email per aggiungerlo al calendario del telefono.


Librerie usate:
1. iCal.Net - Per facilitare la generazione del calendario.
2. PdfPig - Per estrarre le immagini dai pdf.

---

### ENG - Script description

## Add lesson schedules to your phone calendar 📅

This script performs the following steps:

1. **URL generation**: Creates a URL to download the `.pdf` file with the current week's schedule. If it’s Saturday or Sunday, it downloads the next week's schedule.

2. **Image extraction from PDF**: Since the file contains no text, it extracts the image of the schedule from the PDF.

3. **Cropping and analysis**: The image is cropped to extract only the schedule, using the school’s color scheme.

4. **Calendar creation**: The script generates a `.ics` file with the lesson timings.

5. **Sending the calendar**: The file is saved locally and sent via email for easy import on the phone's calendar.

libreries used:
1. iCal.Net -  To easaly generate a calendar.
2. PdfPig - To extract the images from pdf's.
