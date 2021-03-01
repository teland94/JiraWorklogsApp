export function downloadToFile<T>(data: any, fileName: string) {
  const url = window.URL.createObjectURL(data);

  const downloadLink = document.createElement('a');
  downloadLink.href = url;
  downloadLink.download = fileName;
  downloadLink.click();

  window.URL.revokeObjectURL(url);
}
