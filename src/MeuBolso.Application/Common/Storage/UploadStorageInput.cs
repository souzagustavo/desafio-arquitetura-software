namespace MeuBolso.Application.Common.Storage
{
    public record UploadStorageInput(string ContainerName, string Filename, byte[] Content);
}
