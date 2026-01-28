import ClientPage from './ClientPage';

export async function generateStaticParams() {
    return [];
}

export default async function DiscussionPage({ params }: { params: Promise<{ discussionId: string }> }) {
    const { discussionId } = await params;
    return <ClientPage discussionId={discussionId} />;
}
