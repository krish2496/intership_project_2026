import ClientPage from './ClientPage';

export async function generateStaticParams() {
    return [];
}

export default async function ClubDetailsPage({ params }: { params: Promise<{ id: string }> }) {
    const { id } = await params;
    return <ClientPage id={id} />;
}
